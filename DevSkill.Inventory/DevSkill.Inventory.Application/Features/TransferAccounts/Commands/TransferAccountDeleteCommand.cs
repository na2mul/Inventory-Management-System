using DevSkill.Inventory.Domain.Features.TransferAccounts.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Commands
{
    public class TransferAccountDeleteCommand : IRequest, ITransferAccountDeleteCommand
    {
        public Guid Id { get; set; }
    }
}
