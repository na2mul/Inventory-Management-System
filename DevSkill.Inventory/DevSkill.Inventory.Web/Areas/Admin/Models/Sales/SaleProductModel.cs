namespace DevSkill.Inventory.Web.Areas.Admin.Models.Sales
{
    public class SaleProductModel
    {
        public Guid ProductId { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public int StockAvailable { get; set; }
        public string SaleType { get; set; }
    }
}
