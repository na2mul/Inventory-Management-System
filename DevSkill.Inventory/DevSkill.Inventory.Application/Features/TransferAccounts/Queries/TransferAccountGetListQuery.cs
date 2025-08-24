using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Features.TransferAccounts.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Dtos.TransferAccountDtos;

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Queries
{
    public class TransferAccountGetListQuery : DataTables, IRequest<(IList<TransferAccountDto>, int, int)>, ITransferAccountGetListQuery
    {       

    }
}