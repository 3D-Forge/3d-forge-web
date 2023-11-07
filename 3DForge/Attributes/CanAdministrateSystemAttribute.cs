using Microsoft.AspNetCore.Authorization;

namespace Backend3DForge.Attributes
{
	public class CanAdministrateSystemAttribute : AuthorizeAttribute
	{
		public CanAdministrateSystemAttribute()
		{
		}
	}
}
