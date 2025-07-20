using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos.SaleDtos
{
    public class SaleItemDto
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
