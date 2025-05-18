namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductSearchModel
    {
        public string Name { get; set; }
        public double? PriceTo { get; set; }
        public double? PriceFrom { get; set; }
        public string Description { get; set; }

    }
}
