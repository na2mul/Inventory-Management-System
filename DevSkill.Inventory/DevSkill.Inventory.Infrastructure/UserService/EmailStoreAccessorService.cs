using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UserService
{    
    public class EmailStoreAccessorService : IEmailStoreAccessorService
    {
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailStoreAccessorService(IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            _userStore = userStore;
            _userManager = userManager;
        }

        public IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }

}
