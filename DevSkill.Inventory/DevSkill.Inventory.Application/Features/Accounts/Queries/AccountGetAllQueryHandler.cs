using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Accounts.Queries
{
    public class AccountGetAllQueryHandler : IRequestHandler<AccountGetAllQuery, IList<Account>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AccountGetAllQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IList<Account>> Handle(AccountGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.AccountRepository.AccountGetByTypeAsync(request.Id);
        }
    }
}
