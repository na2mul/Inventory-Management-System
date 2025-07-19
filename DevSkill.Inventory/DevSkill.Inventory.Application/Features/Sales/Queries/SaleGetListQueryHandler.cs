using AutoMapper;
using DevSkill.Inventory.Application.Features.Sales.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos.SaleDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Queries
{
    public class SaleGetListQueryHandler : IRequestHandler<SaleGetListQuery, (IList<SaleListDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public SaleGetListQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<(IList<SaleListDto>, int, int)> Handle(SaleGetListQuery request,
            CancellationToken cancellationToken)
        {
            var procedureName = "GetSales";

            var result = await _applicationUnitOfWork.SqlUtility.QueryWithStoredProcedureAsync<SaleListDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", request.PageIndex },
                    { "PageSize", request.PageSize == -1 ? int.MaxValue : request.PageSize },
                    { "OrderBy", request.FormatSortExpression(["Id","InvoiceNo","Date","CustomerName","Total", "Paid", "Due", "Status","Id"]) },
                    { "InvoiceNo", string.IsNullOrEmpty(request.SearchItem.InvoiceNo) ? null : request.SearchItem.InvoiceNo },
                    { "TotalPriceFrom", request.SearchItem.TotalPriceFrom.HasValue ? request.SearchItem.TotalPriceFrom : null },
                    { "TotalPriceTo" , request.SearchItem.TotalPriceTo.HasValue ? request.SearchItem.TotalPriceTo : null},
                    { "Status", string.IsNullOrEmpty(request.SearchItem.Status) ? null : request.SearchItem.Status},
                    { "SaleDateFrom", request.SearchItem.SaleDateFrom.HasValue ? request.SearchItem.SaleDateFrom : null },
                    { "SaleDateTo", request.SearchItem.SaleDateTo.HasValue ? request.SearchItem.SaleDateTo : null},
                    { "CustomerName", string.IsNullOrEmpty(request.SearchItem.CustomerName) ? null : request.SearchItem.CustomerName },
                    { "PaidAmount", request.SearchItem.PaidAmount.HasValue ? request.SearchItem.PaidAmount : null},
                    { "DueAmount", request.SearchItem.DueAmount.HasValue ? request.SearchItem.DueAmount : null},

                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
