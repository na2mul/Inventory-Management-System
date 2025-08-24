using DevSkill.Inventory.Domain.Features.TransferAccounts.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Commands
{
    public class TransferAccountAddCommand : IRequest, ITransferAccountAddCommand
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public double TransferAmount { get; set; }
        public string? Note { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
