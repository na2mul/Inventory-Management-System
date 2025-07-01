using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public string? Barcode { get; set; }
        public string? CategoryName { get; set; }
        public int? PurchasePrice { get; set; }
        public int? MRP { get; set; }
        public int? WholesalePrice { get; set; }
        public int? Stock { get; set; }
        public int? LowStock { get; set; }
        public int? DamageStock { get; set; }
        public int? MeasurementUnit { get; set; }
    }
}
