using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.AccountTypes.Queries;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.AccountTypes.Queries
{
    public class AccountTypeGetAllQuery : IRequest<IList<AccountType>>, IAccountTypeGetAllQuery
    {
    }
}
