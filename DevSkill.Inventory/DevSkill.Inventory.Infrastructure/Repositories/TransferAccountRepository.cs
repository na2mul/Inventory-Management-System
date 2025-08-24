using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos.TransferAccountDtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class TransferAccountRepository : Repository<TransferAccount, Guid>, ITransferAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TransferAccountRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public (IList<TransferAccountDto> data, int total, int totalDisplay) GetPagedTransferAccounts(
            int pageIndex,
            int pageSize,
            string? order,
            DataTablesSearch search)
        {
            Expression<Func<TransferAccount, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(search.Value))
            {
                filter = x =>
                    x.TransferDate.ToString().Contains(search.Value) ||
                    x.TransferAmount.ToString().Contains(search.Value) ||
                    (x.FromAccount.AccountType.Name + x.FromAccount.AccountName).Contains(search.Value) ||
                    (x.ToAccount.AccountType.Name + x.ToAccount.AccountName).Contains(search.Value);
            }

            var (entities, total, totalDisplay) = GetDynamic(
                filter,
                order,
                include: q => q
                    .Include(x => x.FromAccount).ThenInclude(f => f.AccountType)
                    .Include(x => x.ToAccount).ThenInclude(t => t.AccountType),
                pageIndex,
                pageSize,
                isTrackingOff: true
            );

            var dtoList = entities.Select(x => new TransferAccountDto
            {
                Id = x.Id,
                TransferDate = x.TransferDate.ToString("yyyy-MM-dd"),
                TransferAmount = x.TransferAmount,
                Note = x.Note,
                FromAccountDisplay = x.FromAccount != null && x.FromAccount.AccountType != null
                    ? $"{x.FromAccount.AccountType.Name} : {x.FromAccount.AccountName}"
                    : "N/A",
                ToAccountDisplay = x.ToAccount != null && x.ToAccount.AccountType != null
                    ? $"{x.ToAccount.AccountType.Name} : {x.ToAccount.AccountName}"
                    : "N/A"
            }).ToList();

            return (dtoList, total, totalDisplay);
        }
    }
}
