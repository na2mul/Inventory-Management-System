using DevSkill.Inventory.Domain.Dtos.TransferAccountDtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ITransferAccountRepository : IRepository<TransferAccount, Guid>
    {
        public (IList<TransferAccountDto> data, int total, int totalDisplay) GetPagedTransferAccounts(
            int pageIndex, int pageSize, string? order, DataTablesSearch search);
    }
}
