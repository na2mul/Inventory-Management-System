using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetQuery : IRequest<Product>
    {
        public Guid Id { get; set; }                
    }
}
