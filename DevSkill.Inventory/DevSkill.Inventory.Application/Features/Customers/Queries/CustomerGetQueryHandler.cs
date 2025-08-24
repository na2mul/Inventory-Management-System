using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomerGetQueryHandler : IRequestHandler<CustomerGetQuery, Customer>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CustomerGetQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<Customer> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        }
    }
}
