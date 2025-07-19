using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models.Products;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.Sales
{
    public class AddSaleModel
    {
        public string? InvoiceNo { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public Guid AccountTypeId { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int NetAmount { get; set; }
        public int PaidAmount { get; set; }
        public int DueAmount { get; set; }
        public double VatAmount { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public string? Terms { get; set; }
        public string? SaleType { get; set; }
        public List<SaleProductModel> Products { get; set; } = new List<SaleProductModel>();
    }
}
