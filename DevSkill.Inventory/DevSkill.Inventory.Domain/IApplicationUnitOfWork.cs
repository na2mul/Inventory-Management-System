using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }

        Task<(IList<Product> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize, string? order, ProductSearchDto search);
    }
}
