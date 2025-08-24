using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetQuery : IRequest<Product>, IProductGetQuery
    {
        public Guid Id { get; set; }                
    }
}
