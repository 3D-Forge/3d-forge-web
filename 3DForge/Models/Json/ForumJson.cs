namespace Backend3DForge.Models.Json
{
	public class ForumJson
	{
		public string ForumName { get; set; }
		public int? UserId { get; set; }
		public ForumPostJson[] Posts { get; set; }
	}
}
