using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductSearchModel
    {
        public string? Name { get; set; }
        public int? PurchasePriceFrom { get; set; }
        public int? PurchasePriceTo { get; set; }
        public string? Barcode { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? MeasurementUnitId { get; set; }
        public SelectList? Categories { get; set; }
        public SelectList? MeasurementUnits { get; set; }
        public int? MRP { get; set; }
        public int? WholesalePrice { get; set; }
        public int? Stock { get; set; }
        public int? LowStock { get; set; }
        public int? DamageStock { get; set; }
       
        



    }
}
