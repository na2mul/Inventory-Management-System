using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IAccountTypeRepository : IRepository<AccountType, Guid>
    {
        public Task<IList<AccountType>> AccountTypeGetAllAsync();
        public bool IsNameDuplicate(string name, Guid? id = null);
    }
}
