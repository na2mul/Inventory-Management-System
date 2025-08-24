using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Customer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public string? CustomerId { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public  bool? Status { get; set; }
        public int? Balance { get; set; }
    }
}
