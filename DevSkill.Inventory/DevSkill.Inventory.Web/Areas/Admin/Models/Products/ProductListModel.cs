using DevSkill.Inventory.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.Products
{
    public class ProductListModel : DataTables
    {
        public ProductSearchModel SearchItem { get; set; }
        
    }
}
