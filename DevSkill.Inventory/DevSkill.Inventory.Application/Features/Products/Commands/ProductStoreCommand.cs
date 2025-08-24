using DevSkill.Inventory.Domain.Features.Products.Commands;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductStoreCommand : IRequest, IProductStoreCommand
    {
        public Guid Id { get; set; }       
        public int? Stock { get; set; }
        
    }
}
