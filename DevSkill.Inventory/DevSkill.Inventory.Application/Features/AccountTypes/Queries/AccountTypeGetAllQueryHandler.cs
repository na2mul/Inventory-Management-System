using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.AccountTypes.Queries
{
    public class AccountTypeGetAllQueryHandler : IRequestHandler<AccountTypeGetAllQuery, IList<AccountType>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AccountTypeGetAllQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IList<AccountType>> Handle(AccountTypeGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.AccountTypeRepository.AccountTypeGetAllAsync();
        }
    }
}
