using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FoodDelivery.Authorization.Requirements
{
    public class IsRiderAuthorizationHandler : AuthorizationHandler<IsRider>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRider requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Rider))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
