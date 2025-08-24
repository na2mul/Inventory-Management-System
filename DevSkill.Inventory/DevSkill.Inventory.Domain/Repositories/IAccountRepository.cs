using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Account, Guid>
    {
        public Task<IList<Account>> AccountGetByTypeAsync(Guid id);
        public bool IsNameDuplicate(string name, Guid? id = null);
    }
}
