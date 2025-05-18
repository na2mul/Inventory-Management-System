using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public ProductAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            if (!_applicationUnitOfWork.ProductRepository.IsNameDuplicate(product.Name, product.Id))
            {
                await _applicationUnitOfWork.ProductRepository.AddAsync(product);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateProductNameException();
        }
    }
}
