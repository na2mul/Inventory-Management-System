using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class TransferAccount : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Account FromAccount { get; set; }
        public Guid ToAccountId { get; set; }
        public Account ToAccount { get; set; }
        public decimal TransferAmount { get; set; }
        public string? Note { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
