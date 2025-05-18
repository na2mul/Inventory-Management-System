using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public ProductSearchModel SearchItem { get; set; }
    }
}
