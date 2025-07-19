using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public (IList<Product>, int, int) GetPagedProducts(int pageIndex,
            int pageSize, string? order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value) ||
                x.Category.CategoryName.Contains(search.Value), order,
                    null, pageIndex, pageSize, true);
        }

        public async Task<(IList<Product>, int, int)> GetPagedProductsAsync(IProductGetListQuery request)
        {
                return await GetDynamicAsync(x => x.Name.Contains(request.Search.Value) || x.Category.CategoryName.Contains(request.Search.Value),
                    request.FormatSortExpression("Name", "PurchasePrice","Description"),
                    null,
                    request.PageIndex,
                    request.PageSize,
                    true);
        }

        public bool IsNameDuplicate(string name, Guid? id = null)
        {
            if (id.HasValue)
                return GetCount(x => x.Id != id.Value && x.Name == name) > 0;
            else
                return GetCount(x => x.Name == name) > 0;
        }

        public async Task<IList<Product>> ProductGetAllAsync()
        {
            return await GetAllAsync();
        }
        public async void GetProductBySaleTypeAsync(Guid id, int saleType)
        {

        }
    }
}
