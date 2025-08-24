using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        public (IList<Customer>, int, int) GetPagedProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search);
        public Task<Customer?> GetLastCustomerAsync();
        public bool IsNameDuplicate(string name, Guid? id = null);
        public Task<IList<Customer>> CustomerGetAllAsync();

    }
}
