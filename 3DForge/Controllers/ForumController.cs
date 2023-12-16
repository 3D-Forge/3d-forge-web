using Backend3DForge.Attributes;
using Backend3DForge.Models;
using Backend3DForge.Requests;
using Backend3DForge.Requests.Administrate;
using Backend3DForge.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace Backend3DForge.Controllers
{
	[Route("api/forum")]
	[ApiController]
	public class ForumController : BaseController
	{
        protected readonly IMemoryCache memoryCache;

		public ForumController(DbApp db, IMemoryCache memoryCache) : base(db)
		{
            this.memoryCache = memoryCache;
		}

		[HttpGet("threads")]
		public async Task<IActionResult> GetThreads([FromQuery] PageRequest request, [FromQuery(Name = "q")] string? q = null)
		{
            PageResponse<ForumThreadResponse.View>? response;
            string key = $"GET api/forum/threads?q={q ?? ""}&p={request.Page}&ps={request.PageSize}";
            if (!memoryCache.TryGetValue(key, out response))
			{
				var query = DB.ForumThreads
					.Include(x => x.Posts)
					.OrderByDescending(x => x.CreatedAt)
					.AsQueryable();

				IEnumerable<int> includedPosts = Enumerable.Empty<int>();

				if (q != null)
				{
					var fromNames = query.Where(p => p.ForumName.Contains(q));
					var fromPosts = query.Where(p => p.Posts.Any(x => x.PostText.Contains(q)));
					includedPosts = fromPosts.SelectMany(x => x.Posts.Where(x => x.PostText.Contains(q)).Select(y=>y.Id)).ToList();
					query = fromNames.Union(fromPosts);
				}

				int totalItemsCount = await query.CountAsync();
                int totalPagesCount = (int)Math.Ceiling((double)totalItemsCount / request.PageSize);
				query = query
					.Skip((request.Page - 1) * request.PageSize)
					.Take(request.PageSize);

                var result = await query.Select(p => new ForumThreadResponse.View(p)
				{
					PostIds = p.Posts
						.Where(x => includedPosts.Contains(x.Id))
						.Select(x => new PostResponse.View(x))
				}).ToListAsync();
				response = new PageResponse<ForumThreadResponse.View>(
					result,
					request.Page,
					request.PageSize,
					totalPagesCount);

				memoryCache.Set(key, response, TimeSpan.FromSeconds(45));
			}
			return Ok(response);
		}

		[HttpGet("thread/{forumThreadId}/posts")]
		public async Task<IActionResult> GetPosts([FromRoute] int forumThreadId, [FromQuery] PageRequest request, [FromQuery(Name = "q")] string? q = null)
		{
			PageResponse<PostResponse.View>? response;
			string key = $"GET api/forum/thread/{forumThreadId}/posts?q={q ?? ""}&p={request.Page}&ps={request.PageSize}";
			if (!memoryCache.TryGetValue(key, out response))
			{
				var query = DB.Posts
					.Where(x => x.ForumThreadId == forumThreadId)
					.OrderBy(x => x.CreateAt)
					.AsQueryable();

				if (q != null)
				{
					query = query.Where(p => p.PostText.Contains(q));
				}

				int totalItemsCount = await query.CountAsync();
				int totalPagesCount = (int)Math.Ceiling((double)totalItemsCount / request.PageSize);
				query = query
					.Skip((request.Page - 1) * request.PageSize)
					.Take(request.PageSize);

				var result = await query.Select(p => new PostResponse.View(p)).ToListAsync();
				response = new PageResponse<PostResponse.View>(
					result,
					request.Page,
					request.PageSize,
					totalPagesCount);

				memoryCache.Set(key, response, TimeSpan.FromSeconds(45));
			}
			return Ok(response);
		}

		[HttpGet("thread/{forumThreadId}/post/{postId}")]
		public async Task<IActionResult> GetPost([FromRoute] int forumThreadId, [FromRoute] int postId)
		{
			PostResponse? response;
			string key = $"GET api/forum/thread/{forumThreadId}/post/{postId}";
			if (!memoryCache.TryGetValue(key, out response))
			{
				var post = await DB.Posts
					.FirstOrDefaultAsync(x => x.ForumThreadId == forumThreadId && x.Id == postId);

				if (post is null)
				{
					return BadRequest("Selected post not found");
				}

				response = new PostResponse(post);

				memoryCache.Set(key, response, TimeSpan.FromSeconds(45));
			}
			return Ok(response);
		}

		[Authorize]
		[HttpPost("thread")]
		public async Task<IActionResult> CreateThread([FromBody] CreateThreadRequest request)
		{
			if (AuthorizedUser.Blocked)
			{
				return StatusCode(403, new BaseResponse.ErrorResponse("User is blocked"));
			}

			if (await DB.ForumThreads.AnyAsync(x => x.ForumName == request.ForumName))
			{
				return BadRequest(new BaseResponse.ErrorResponse("Forum name is used"));
			}

			if (await DB.BannedWords.AnyAsync(x => request.ForumName.Contains(x.BannedWordName)))
			{
				return BadRequest(new BaseResponse.ErrorResponse("Forum name contains banned word(s)"));
			}

			var thread = new ForumThread()
			{
				ForumName = request.ForumName,
				CreatedAt = DateTime.UtcNow,
				UserId = AuthorizedUserId,
			};

			thread = (await DB.ForumThreads.AddAsync(thread)).Entity;

			await DB.SaveChangesAsync();

			if (request.Post is not null)
			{
				var post = new Post()
				{
					ForumThreadId = thread.Id,
					PostText = request.Post.Text,
					CreateAt = DateTime.UtcNow,
					UserId = AuthorizedUserId,
				};

				var bannedWords = await DB.BannedWords.ToListAsync();

				if (bannedWords.Any(x => Regex.IsMatch(post.PostText, x.BannedWordName)))
				{
					post.ContainsAbuseContent = true;
				}

				DB.Posts.Add(post);
			}

			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Forum thread succesfully created", new ForumThreadResponse(thread)));
		}

		[Authorize]
		[HttpPost("post")]
		public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
		{
			if (!await DB.ForumThreads.AnyAsync(th => th.Id == request.ThreadId))
			{
				return BadRequest(new BaseResponse.ErrorResponse("Selected forum thread not found"));
			}

			if (request.ReplayPostId is not null && !await DB.Posts.AnyAsync(p => p.Id == request.ReplayPostId))
			{
				return BadRequest(new BaseResponse.ErrorResponse("Selected request post not found"));
			}

			var post = new Post()
			{
				ForumThreadId = request.ThreadId,
				PostText = request.Text,
				CreateAt = DateTime.UtcNow,
				UserId = AuthorizedUserId,
				ReplayPostId = request.ReplayPostId,
			};

			var bannedWords = await DB.BannedWords.ToListAsync();

			if (bannedWords.Any(x => Regex.IsMatch(post.PostText, x.BannedWordName)))
			{
				post.ContainsAbuseContent = true;
			}

			post = (await DB.Posts.AddAsync(post)).Entity;
			await DB.SaveChangesAsync();

			return Ok(new PostResponse(post));
		}
		
		[Authorize]
		[HttpPut("post")]
		public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request)
		{
			var post = await DB.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);

			if (post is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Selected post not found"));
			}

			var bannedWords = await DB.BannedWords.ToListAsync();

			if (bannedWords.Any(x => Regex.IsMatch(post.PostText, x.BannedWordName)))
			{
				post.ContainsAbuseContent = true;
			}

			post.PostText = request.Text;
			post.EditedAt = DateTime.UtcNow;

			post = DB.Posts.Update(post).Entity;
			await DB.SaveChangesAsync();

			return Ok(new PostResponse(post));
		}
		
		[CanAdministrateForum]
		[HttpPut("markAbusivePost")]
		public async Task<IActionResult> MarkAbusivePost([FromBody] UpdateAbusivePostRequest request)
		{
			var post = await DB.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);

			if (post is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Selected post not found"));
			}

			post.ContainsAbuseContent = request.ContainsAbusive;

			post = DB.Posts.Update(post).Entity;
			await DB.SaveChangesAsync();

			return Ok(new PostResponse(post));
		}

		[CanAdministrateForum]
		[HttpDelete("thread")]
		public async Task<IActionResult> DeleteThread([FromBody] int threadId)
		{
			var thread = await DB.ForumThreads.FirstOrDefaultAsync(p => p.Id == threadId);

			if (thread is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Selected post not found"));
			}

			var posts = DB.Posts.Where(p => p.ForumThreadId == threadId);

			DB.Posts.RemoveRange(posts);

			DB.ForumThreads.Remove(thread);
			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Thread has been deleted"));
		}

		[Authorize]
		[HttpDelete("post")]
		public async Task<IActionResult> DeletePost([FromBody] int postId)
		{
			var post = await DB.Posts
				.Include(p => p.User)
				.FirstOrDefaultAsync(x => x.Id == postId);

			if (post is null)
			{
				return NotFound(new BaseResponse.ErrorResponse("Selected post not found"));
			}

			if (post.UserId != AuthorizedUserId && !AuthorizedUser.CanAdministrateForum)
			{
				return StatusCode(403, new BaseResponse.ErrorResponse("You are not allowed to delete this post"));
			}

			DB.Posts.Remove(post);
			await DB.SaveChangesAsync();

			return Ok(new BaseResponse.SuccessResponse("Post has been deleted"));
		}
	}
}
