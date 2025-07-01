using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        //public async Task<(IList<Category>, int, int)> GetPagedCategoriesAsync(ICategoryGetListQuery request)
        //{
        //    return await GetDynamicAsync(x => x.Name.Contains(request.Search.Value) || x.Description.Contains(request.Search.Value),
        //        request.FormatSortExpression("Name", "PurchasePrice", "Description"),
        //        null,
        //        request.PageIndex,
        //        request.PageSize,
        //        true);
        //}

        public async Task<IList<Category>> GetOrderedCategoriesAsync()
        {
            return await GetAsync(null, x => x.OrderBy(y => y.CategoryName), null, true);
        }
    }
}
