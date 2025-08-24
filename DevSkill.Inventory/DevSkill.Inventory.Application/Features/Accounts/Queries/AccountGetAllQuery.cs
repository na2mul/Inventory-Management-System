using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Accounts.Queries;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Accounts.Queries
{
    public class AccountGetAllQuery : IRequest<IList<Account>>, IAccountGetAllQuery
    {
        public Guid Id { get; set; }
    }
}
