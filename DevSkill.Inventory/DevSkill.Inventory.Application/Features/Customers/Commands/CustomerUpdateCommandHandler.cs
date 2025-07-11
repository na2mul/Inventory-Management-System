using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Exceptions;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommandHandler : IRequestHandler<CustomerUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CustomerUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);
            if (!_applicationUnitOfWork.CustomerRepository.IsNameDuplicate(customer.Name, customer.Id))
            {
                _applicationUnitOfWork.CustomerRepository.Update(customer);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateNameException("Customer");

        }
    }
}
