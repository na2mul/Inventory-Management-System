using DevSkill.Inventory.Domain.Features.Products.Commands;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDamageCommand : IRequest, IProductDamageCommand
    {
        public Guid Id { get; set; }       
        public int? Stock { get; set; }
    }
}
