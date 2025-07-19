using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos.SaleDtos
{
    public class SaleSearchDto
    {
        public string? InvoiceNo { get; set; }
        public DateTime? SaleDateFrom { get; set; }
        public DateTime? SaleDateTo { get; set; }
        public string? CustomerName { get; set; }
        public int? TotalPriceFrom { get; set; }
        public int? TotalPriceTo { get; set; }
        public int? PaidAmount { get; set; }
        public int? DueAmount { get; set; }
        public string? Status { get; set; }
    }
}
