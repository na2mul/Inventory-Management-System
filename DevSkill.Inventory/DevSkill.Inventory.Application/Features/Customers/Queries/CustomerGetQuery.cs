using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomerGetQuery : IRequest<Customer>, ICustomerGetQuery
    {
        public Guid Id { get; set; }
    }
}
