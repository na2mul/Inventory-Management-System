using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; set; }
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUnitOfWork(ApplicationDbContext context, IProductRepository productRepository)
            : base(context)
        {
            _dbContext = context;
            ProductRepository = productRepository;
        }

        public async Task<(IList<Product> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
            string? order, ProductSearchDto search)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<Product>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "PriceFrom", search.PriceFrom },
                    { "PriceTo", search.PriceTo },
                    { "Name", string.IsNullOrEmpty(search.Name) ? null : search.Name },
                    { "Description", string.IsNullOrEmpty(search.Description) ? null : search.Description }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
