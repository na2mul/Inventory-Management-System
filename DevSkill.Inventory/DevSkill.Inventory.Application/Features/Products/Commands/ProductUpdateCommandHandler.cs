using AutoMapper;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using DevSkill.Inventory.Application.Exceptions;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public ProductUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            if (!_applicationUnitOfWork.ProductRepository.IsNameDuplicate(product.Name, product.Id))
            {
                _applicationUnitOfWork.ProductRepository.Update(product);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateProductNameException();
            
        }
    }
}
