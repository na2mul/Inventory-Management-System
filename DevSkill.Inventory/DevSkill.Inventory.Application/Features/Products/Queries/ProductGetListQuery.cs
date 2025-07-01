using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetListQuery : DataTables, IRequest<(IList<ProductListDto>, int, int)>, IProductGetListQuery
    {
        public ProductSearchDto SearchItem { get; set; }
        
    }
}
