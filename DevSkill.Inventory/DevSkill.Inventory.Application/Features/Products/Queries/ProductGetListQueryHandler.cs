using AutoMapper;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetListQueryHandler : IRequestHandler<ProductGetListQuery, (IList<ProductListDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public ProductGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<(IList<ProductListDto>, int, int)> Handle(ProductGetListQuery request,
            CancellationToken cancellationToken)
        {
            var search = _mapper.Map<ProductSearchDto>(request.SearchItem);
            var procedureName = "GetProducts";

            var result = await _applicationUnitOfWork.SqlUtility.QueryWithStoredProcedureAsync<ProductListDto>(procedureName,
            new Dictionary<string, object>
            {
                { "PageIndex", request.PageIndex },
                { "PageSize", request.PageSize },
                { "OrderBy", request.FormatSortExpression(["Name","PurchasePrice","Barcode","CategoryName","MRP","WholesalePrice","Id"]) },
                { "PriceFrom", search.PriceFrom },
                { "PriceTo", search.PriceTo },
                { "Name", string.IsNullOrEmpty(search.Name) ? null : search.Name }
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
