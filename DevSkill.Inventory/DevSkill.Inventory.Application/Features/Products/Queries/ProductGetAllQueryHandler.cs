using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetAllQueryHandler : IRequestHandler<ProductGetAllQuery, IList<Product>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ProductGetAllQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IList<Product>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository.ProductGetAllAsync();
        }
    }
}
