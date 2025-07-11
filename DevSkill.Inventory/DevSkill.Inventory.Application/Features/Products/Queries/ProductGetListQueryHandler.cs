using AutoMapper;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Domain.Dtos.ProductDtos;

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
            var procedureName = "GetProducts";

            var result = await _applicationUnitOfWork.SqlUtility.QueryWithStoredProcedureAsync<ProductListDto>(procedureName,                
                new Dictionary<string, object>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["Id","ImageUrl","Name","Barcode","CategoryName", "PurchasePrice", "MRP", "WholesalePrice", "Stock","LowStock","DamageStock","Id"]) },
                    { "Name", string.IsNullOrEmpty(request.SearchItem.Name) ? null : request.SearchItem.Name },
                    { "PurchasePriceFrom", request.SearchItem.PurchasePriceFrom.HasValue ? request.SearchItem.PurchasePriceFrom : null },
                    { "PurchasePriceTo", request.SearchItem.PurchasePriceTo.HasValue ? request.SearchItem.PurchasePriceTo : null},
                    { "Barcode", string.IsNullOrEmpty(request.SearchItem.Barcode) ? null : request.SearchItem.Barcode},
                    { "StockFrom", request.SearchItem.StockFrom.HasValue ? request.SearchItem.StockFrom : null },
                    { "StockTo", request.SearchItem.StockTo.HasValue ? request.SearchItem.StockTo : null},
                    { "CategoryName", string.IsNullOrEmpty(request.SearchItem.CategoryName) ? null : request.SearchItem.CategoryName },
                    { "MeasurementUnitName", string.IsNullOrEmpty(request.SearchItem.MeasurementUnitName) ? null : request.SearchItem.MeasurementUnitName }
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
