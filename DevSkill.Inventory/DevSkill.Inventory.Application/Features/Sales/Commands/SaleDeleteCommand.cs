using DevSkill.Inventory.Domain.Features.Sales.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleDeleteCommand : IRequest, ISaleDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
