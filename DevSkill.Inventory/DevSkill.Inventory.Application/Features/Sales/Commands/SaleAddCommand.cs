using DevSkill.Inventory.Domain.Dtos.SaleDtos;
using DevSkill.Inventory.Domain.Features.Sales.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleAddCommand : IRequest, ISaleAddCommand
    {
        public Guid Id { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int NetAmount { get; set; }
        public int PaidAmount { get; set; }
        public int DueAmount { get; set; }
        public double VatAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
