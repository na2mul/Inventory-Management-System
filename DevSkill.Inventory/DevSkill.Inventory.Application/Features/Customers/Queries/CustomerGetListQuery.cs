using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Features.Customers.Queries;
using DevSkill.Inventory.Domain.Dtos.ProductDtos;
using DevSkill.Inventory.Domain.Dtos.CustomerDtos;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomerGetListQuery : DataTables, IRequest<(IList<CustomerListDto>, int, int)>, ICustomerGetListQuery
    {
        public CustomerSearchDto SearchItem { get; set; }

    }
}
