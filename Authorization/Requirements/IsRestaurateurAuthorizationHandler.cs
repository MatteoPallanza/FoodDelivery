using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FoodDelivery.Authorization.Requirements
{
    public class IsRestaurateurAuthorizationHandler : AuthorizationHandler<IsRestaurateur>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRestaurateur requirement)
        {
            if (context.User.HasClaim(ClaimName.Role, RoleName.Restaurateur))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
