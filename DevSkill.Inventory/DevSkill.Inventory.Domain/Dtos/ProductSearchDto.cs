using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductSearchDto
    {
        public string? Name { get; set; }
        public int? PurchasePriceFrom { get; set; }
        public int? PurchasePriceTo { get; set; }
        public int? StockFrom { get; set; }
        public int? StockTo { get; set; }
        public string? Barcode { get; set; }
        public string? CategoryName { get; set; }
        public string? MeasurementUnitName { get; set; }
    }
}
