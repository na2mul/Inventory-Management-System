using DevSkill.Inventory.Domain.Features.Products.Commands;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDeleteCommand : IRequest, IProductDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
