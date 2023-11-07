using Microsoft.AspNetCore.Authorization;

namespace Backend3DForge.Attributes
{
	public class CanAdministrateForumAttribute : AuthorizeAttribute
	{
		public CanAdministrateForumAttribute()
		{
		}
	}
}
