using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category,Guid>
    {
        public Task<IList<Category>> GetOrderedCategoriesAsync();
    }
}
