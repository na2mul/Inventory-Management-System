using AutoMapper;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDamageCommandHandler : IRequestHandler<ProductDamageCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public ProductDamageCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }
        public async Task Handle(ProductDamageCommand request, CancellationToken cancellationToken)
        {
            var productCopy = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);
            productCopy.Stock -= request.Stock;
            productCopy.DamageStock += request.Stock;
            var product = _mapper.Map<Product>(productCopy);            
            _applicationUnitOfWork.ProductRepository.Update(product);
            await _applicationUnitOfWork.SaveAsync();         
        }
    }
}
