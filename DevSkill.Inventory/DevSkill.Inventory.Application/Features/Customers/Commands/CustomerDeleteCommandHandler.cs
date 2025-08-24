using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerDeleteCommandHandler : IRequestHandler<CustomerDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CustomerDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.CustomerRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
