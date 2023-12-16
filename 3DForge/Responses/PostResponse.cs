using Backend3DForge.Models;

namespace Backend3DForge.Responses
{
	public class PostResponse : BaseResponse
	{
		public PostResponse(Post post) : base(true, null, null)
		{
			Data = new View(post);
		}

		public PostResponse(ICollection<Post> posts) : base(true, null, null)
		{
			Data = posts.Select(post => new View(post));
		}

		public class View
		{
			public int Id { get; set; }
			public string PostText { get; set; }
			public DateTime CreateAt { get; set; }
			public int? UserId { get; set; }
			public bool ContainsAbuseContent { get; set; }
			public int? ReplayPostId { get; set; }
			public DateTime EditedAt { get; set; }

			public View(Post post)
			{
				Id = post.Id;
				PostText = post.PostText;
				CreateAt = post.CreateAt;
				UserId = post.UserId;
				ContainsAbuseContent = post.ContainsAbuseContent;
				ReplayPostId = post.ReplayPostId;
				EditedAt = post.EditedAt;
			}
		}
	}
}
