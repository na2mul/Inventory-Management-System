using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class SalesDetail : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid SalesId { get; set; }
        public Sale? Sale { get; set; }
        public Guid ProductId { get; set; }       
        public Product? Product { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string SaleType { get; set; }
        public DateTime CreatedAt { get; set; }               
    }
}
