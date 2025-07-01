using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class UpdateProductModel
    {
        public Guid Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? PurchasePrice { get; set; }
        public string? MRP { get; set; }
        public string? WholesalePrice { get; set; }
        public string? Stock { get; set; }
        public string LowStock { get; set; }
        public string DamageStock { get; set; }
        public Guid? MeasurementUnitId { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
    }
}
