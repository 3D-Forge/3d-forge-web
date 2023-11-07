using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend3DForge.Attributes
{
	public class CanRetrieveDeliveryHandler : AuthorizationHandler<CanRetrieveDeliveryRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanRetrieveDeliveryRequirement requirement)
		{
			if (context.User.FindFirstValue("can_retrieve_delivery") == true.ToString())
			{
				context.Succeed(requirement);
			}
			context.Fail();
			return Task.CompletedTask;
		}
	}
}
