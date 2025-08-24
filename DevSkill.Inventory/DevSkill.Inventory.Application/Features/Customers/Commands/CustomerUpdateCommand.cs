using DevSkill.Inventory.Domain.Features.Customers.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommand : IRequest, ICustomerUpdateCommand
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public string? CustomerId { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public bool? Status { get; set; }
        public int? Balance { get; set; }
    }
}
