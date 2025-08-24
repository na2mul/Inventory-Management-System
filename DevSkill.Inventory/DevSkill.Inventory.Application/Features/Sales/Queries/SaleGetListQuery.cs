
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos.SaleDtos;
using DevSkill.Inventory.Domain.Features.Sales.Queries;

namespace DevSkill.Inventory.Application.Features.Sales.Queries
{
    public class SaleGetListQuery : DataTables, IRequest<(IList<SaleListDto>, int, int)>, ISaleGetListQuery
    {
        public SaleSearchDto SearchItem { get; set; }

    }
}
