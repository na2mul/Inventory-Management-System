using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Infrastructure.Identity;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public static class ClaimSeed
    {
        public static ApplicationUserClaim[] GetClaims()
        {
            return [ 
                new ApplicationUserClaim 
                { 
                    Id = -1,
                    UserId = new Guid("E5FAC9C3-33A8-494B-A31A-5805D9494F69"), 
                    ClaimType = "create_user", 
                    ClaimValue = "allowed" 
                },
            ];
        }
    }
}
