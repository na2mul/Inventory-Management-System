using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Account : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? AccountName { get; set; }
        public Guid AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
    }
}
