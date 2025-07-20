using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos.TransferAccountDtos
{
    public class TransferAccountDto
    {
        public Guid Id { get; set; }
        public string TransferDate { get; set; }
        public decimal TransferAmount { get; set; }
        public string FromAccountDisplay { get; set; }
        public string ToAccountDisplay { get; set; }
        public string Note { get; set; }
    }
}