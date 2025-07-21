using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Commands
{
    public class TransferAccountDeleteCommandHandler : IRequestHandler<TransferAccountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public TransferAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task Handle(TransferAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.TransferAccountRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
