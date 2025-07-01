using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class AddProductModel
    {
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; set; }
        public int? PurchasePrice { get; set; }
        public int? MRP { get; set; }
        public int? WholesalePrice { get; set; }
        public int? Stock { get; set; }
        public int? LowStock { get; set; }
        public int? DamageStock { get; set; }
        public Guid? MeasurementUnitId { get; set; }
        public IList<SelectListItem>? MeasurementUnit { get; set; }
    }
}
