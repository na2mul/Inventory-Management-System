using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos.ProductDtos;
using DevSkill.Inventory.Domain.Dtos.CustomerDtos;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomerGetListQueryHandler : IRequestHandler<CustomerGetListQuery, (IList<CustomerListDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CustomerGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<(IList<CustomerListDto>, int, int)> Handle(CustomerGetListQuery request,
            CancellationToken cancellationToken)
        {
            var procedureName = "GetCustomers";

            var result = await _applicationUnitOfWork.SqlUtility.QueryWithStoredProcedureAsync<CustomerListDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["Id","ImageUrl","ID","Name","Mobile","Address", "Email", "Balance", "Status","Id"]) },
                    { "Name", string.IsNullOrEmpty(request.SearchItem.Name) ? null : request.SearchItem.Name },
                    { "BalanceFrom", request.SearchItem.BalanceFrom.HasValue ? request.SearchItem.BalanceFrom : null },
                    { "BalanceTo", request.SearchItem.BalanceTo.HasValue ? request.SearchItem.BalanceTo : null},
                    { "Mobile", string.IsNullOrEmpty(request.SearchItem.Mobile) ? null : request.SearchItem.Mobile},
                    { "Address", string.IsNullOrEmpty(request.SearchItem.Address) ? null : request.SearchItem.Address},
                    { "Email", string.IsNullOrEmpty(request.SearchItem.Email) ? null : request.SearchItem.Email},
                    { "CustomerId", string.IsNullOrEmpty(request.SearchItem.CustomerId) ? null : request.SearchItem.CustomerId}
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
