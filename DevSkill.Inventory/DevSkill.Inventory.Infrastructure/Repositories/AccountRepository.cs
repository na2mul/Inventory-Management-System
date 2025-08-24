using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account, Guid>, IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public bool IsNameDuplicate(string name, Guid? id = null)
        {
            if (id.HasValue)
                return GetCount(x => x.Id != id.Value && x.AccountName == name) > 0;
            else
                return GetCount(x => x.AccountName == name) > 0;
        }
        public async Task<IList<Account>> AccountGetByTypeAsync(Guid accountTypeId)
        {
            return await GetAsync(x => x.AccountTypeId == accountTypeId, null);
        }


    }
}
