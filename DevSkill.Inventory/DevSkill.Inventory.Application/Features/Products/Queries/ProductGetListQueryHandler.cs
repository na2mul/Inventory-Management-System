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
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetListQueryHandler : IRequestHandler<ProductGetListQuery, (IList<Product>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public ProductGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<(IList<Product>, int, int)> Handle(ProductGetListQuery request,
            CancellationToken cancellationToken)
        {
            var searchDto = _mapper.Map<ProductSearchDto>(request.SearchItem);
            return await _applicationUnitOfWork.GetProductsSP(
                request.PageIndex,
                request.PageSize,
                request.FormatSortExpression("Name","Price","StockQuantity","Description","Id"), searchDto);
            
        }
    }
}
