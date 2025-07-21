using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos.TransferAccountDtos;

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Queries
{
    public class TransferAccountGetListQueryHandler : IRequestHandler<TransferAccountGetListQuery, (IList<TransferAccountDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public TransferAccountGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public async Task<(IList<TransferAccountDto>, int, int)> Handle(TransferAccountGetListQuery request,
            CancellationToken cancellationToken)
        {
            return _applicationUnitOfWork.TransferAccountRepository.GetPagedTransferAccounts(request.PageIndex, request.PageSize == -1 ? int.MaxValue : request.PageSize,
                    request.FormatSortExpression("Id", "TransferDate", "FromAccount", "ToAccount", "TransferAmount", "Note", "Id"), request.Search);
        }
    }
}
