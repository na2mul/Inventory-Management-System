using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public (IList<Customer>, int, int) GetPagedProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value), order,
                    null, pageIndex, pageSize, true);
        }

        public async Task<Customer?> GetLastCustomerAsync()
        {
            return await _dbContext.Customers
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();
        }


        public bool IsNameDuplicate(string name, Guid? id = null)
        {
            if (id.HasValue)
                return GetCount(x => x.Id != id.Value && x.Name == name) > 0;
            else
                return GetCount(x => x.Name == name) > 0;
        }

        public async Task<IList<Customer>> CustomerGetAllAsync()
        {
            return await GetAllAsync();
        }
    }
}
