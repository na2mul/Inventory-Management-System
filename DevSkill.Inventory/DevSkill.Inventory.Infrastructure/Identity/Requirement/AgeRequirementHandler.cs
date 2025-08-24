using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Infrastructure.Identity.Requirement
{
    public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            AgeRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "age" &&
                int.Parse(x.Value) > 20 && int.Parse(x.Value) < 40))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
