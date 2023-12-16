using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
	public class ForumThreadResponse : BaseResponse
	{
		public ForumThreadResponse(ForumThread forumThread) : base(true, null, null)
		{
			Data = new View(forumThread);
		}

		public ForumThreadResponse(ICollection<ForumThread> forumThreads) : base(true, null, null)
		{
			Data = forumThreads.Select(ft => new View(ft));
		}

		public class View
		{
			public int Id { get; set; }
			public string ForumName { get; set; }
			public DateTime CreatedAt { get; set; }
			public int? CreatorId { get; set; }
			public IEnumerable<PostResponse.View>? PostIds { get; set; }

			public View(ForumThread forumThread)
			{
				Id = forumThread.Id;
				ForumName = forumThread.ForumName;
				CreatedAt = forumThread.CreatedAt;
				CreatorId = forumThread.UserId;
				PostIds = forumThread.Posts.Select(x => new PostResponse.View(x));
			}
		}
	}
}
