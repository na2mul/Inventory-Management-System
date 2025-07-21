using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.TransferAccounts
{
    public class AddTransferAccountModel
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public double TransferAmount { get; set; }
        public string? Note { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
