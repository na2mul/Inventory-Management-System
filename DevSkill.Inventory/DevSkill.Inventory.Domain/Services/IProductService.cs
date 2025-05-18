using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Services
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void DeleteProduct(Guid id);
        Product GetProduct(Guid id);
        (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize,
            string? order, DataTablesSearch search);
        void Update(Product product);
    }
}
