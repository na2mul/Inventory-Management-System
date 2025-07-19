using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class AccountTypeRepository : Repository<AccountType, Guid>, IAccountTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountTypeRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }      
        public bool IsNameDuplicate(string name, Guid? id = null)
        {
            if (id.HasValue)
                return GetCount(x => x.Id != id.Value && x.Name == name) > 0;
            else
                return GetCount(x => x.Name == name) > 0;
        }

        public async Task<IList<AccountType>> AccountTypeGetAllAsync()
        {
            return await GetAllAsync();
        }
    }
}
