using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend3DForge.Attributes
{
	public class CanAdministrateForumHandler : AuthorizationHandler<CanAdministrateForumRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAdministrateForumRequirement requirement)
		{
			if (context.User.FindFirstValue("can_administrate_forum") == true.ToString())
			{
				context.Succeed(requirement);
			}
			context.Fail();
			return Task.CompletedTask;
		}
	}
}
