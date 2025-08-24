namespace DevSkill.Inventory.Web.Areas.Admin.Models.Customers
{
    public class UpdateCustomerModel
    {
        public Guid Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? Name { get; set; }
        public string? CustomerId { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public bool? Status { get; set; }
        public int? Balance { get; set; }
    }
}
