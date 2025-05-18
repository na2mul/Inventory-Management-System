using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetListQueryHandler : IRequestHandler<ProductGetListQuery, (IList<Product>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ProductGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<Product>, int, int)> Handle(ProductGetListQuery request,
            CancellationToken cancellationToken)
        {
             return await _applicationUnitOfWork.ProductRepository.GetPagedProductsAsync(request);
            
        }
    }
}
