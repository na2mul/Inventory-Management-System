using DevSkill.Inventory.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public static class RoleSeed
    {
        public static ApplicationRole[] GetRoles()
        {
            return [
                new ApplicationRole
                {
                    Id = new Guid("5005CC5D-736D-4F70-98D0-903749A52B86"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 1).ToString(),
                },
                new ApplicationRole
                {
                    Id = new Guid("28B143BC-7EBA-48E0-985F-AD23853D34F8"),
                    Name = "HR",
                    NormalizedName = "HR",
                    ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 3).ToString(),
                },
                new ApplicationRole
                {
                    Id = new Guid("77FB31EC-AFC0-44E1-ACDC-16D14631D805"),
                    Name = "PublicUser",
                    NormalizedName = "PUBLICUSER",
                    ConcurrencyStamp = new DateTime(2025, 4, 19, 1, 2, 4).ToString(),
                }
            ];
        }
    }
}
