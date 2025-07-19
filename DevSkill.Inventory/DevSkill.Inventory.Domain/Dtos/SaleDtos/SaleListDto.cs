using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos.SaleDtos
{
    public class SaleListDto
    {
        public Guid Id { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public string? CustomerName { get; set; }
        public string? AccountName { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int NetAmount { get; set; }
        public int PaidAmount { get; set; }
        public int DueAmount { get; set; }
        public double Vat { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
