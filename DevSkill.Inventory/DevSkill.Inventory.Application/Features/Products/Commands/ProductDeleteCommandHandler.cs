using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ProductDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.ProductRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
