using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos.CustomerDtos
{
    public class CustomerSearchDto
    {
        public string? Name { get; set; }
        public int? BalanceFrom { get; set; }
        public int? BalanceTo { get; set; }
        public string? CustomerId { get; set; }
        public string? Address { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
    }
}
