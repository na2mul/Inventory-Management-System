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
using System.Text.RegularExpressions;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CustomerAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);

            var lastCustomer = await _applicationUnitOfWork.CustomerRepository.GetLastCustomerAsync();
            int lastNumber = 0;
            if (lastCustomer != null && !string.IsNullOrEmpty(lastCustomer.CustomerId))
            {
                var match = Regex.Match(lastCustomer.CustomerId, @"C-DEV(\d+)");
                if (match.Success)
                {
                    lastNumber = int.Parse(match.Groups[1].Value);
                }
            }
            customer.CustomerId = $"C-DEV{(lastNumber + 1).ToString("D5")}";

            if (!_applicationUnitOfWork.CustomerRepository.IsNameDuplicate(customer.Name, customer.Id))
            {
                await _applicationUnitOfWork.CustomerRepository.AddAsync(customer);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateNameException("Customer");
        }
    }
}
