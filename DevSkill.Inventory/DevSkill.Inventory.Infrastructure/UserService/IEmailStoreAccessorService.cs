using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UserService
{
    public interface IEmailStoreAccessorService
    {
        IUserEmailStore<ApplicationUser> GetEmailStore();
    }
}
