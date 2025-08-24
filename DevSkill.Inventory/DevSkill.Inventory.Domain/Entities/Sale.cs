using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Sale : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }        
        public int TotalAmount { get; set; }         
        public int Discount { get; set; }            
        public int NetAmount { get; set; }           
        public int PaidAmount { get; set; }
        public int DueAmount { get; set; }
        public double VatAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public List<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    }
}
