using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        (IList<Product>, int, int) GetPagedProducts(int pageIndex, int pageSize, string? order, DataTablesSearch search);
        Task<(IList<Product>, int, int)> GetPagedProductsAsync(IProductGetListQuery request);
        bool IsNameDuplicate(string name, Guid? id = null);
    }
}
