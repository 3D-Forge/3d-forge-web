using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend3DForge.Attributes
{
	public class CanModerateCatalogHandler : AuthorizationHandler<CanModerateCatalogRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanModerateCatalogRequirement requirement)
		{
			if (context.User.FindFirstValue("can_moderate_catalog") == true.ToString())
			{
				context.Succeed(requirement);
			}
			context.Fail();
			return Task.CompletedTask;
		}
	}
}
