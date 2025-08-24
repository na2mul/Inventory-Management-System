using DevSkill.Inventory.Domain.Features.Customers.Commands;
using DevSkill.Inventory.Domain.Features.Products.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerDeleteCommand : IRequest, ICustomerDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
