using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend3DForge.Attributes
{
	public class CanAdministrateSystemHandler : AuthorizationHandler<CanAdministrateSystemRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanAdministrateSystemRequirement requirement)
		{
			if (context.User.FindFirstValue("can_administrate_system") == true.ToString())
			{
				context.Succeed(requirement);
			}
			context.Fail();
			return Task.CompletedTask;
		}
	}
}
