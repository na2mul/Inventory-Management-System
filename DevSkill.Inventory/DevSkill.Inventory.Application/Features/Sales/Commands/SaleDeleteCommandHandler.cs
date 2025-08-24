using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleDeleteCommandHandler : IRequestHandler<SaleDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public SaleDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(SaleDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.SaleRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
