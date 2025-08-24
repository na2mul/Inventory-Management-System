using DevSkill.Inventory.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UserService
{
    public interface ICreateUserService
    {
        public ApplicationUser CreateUser();
    }
}
