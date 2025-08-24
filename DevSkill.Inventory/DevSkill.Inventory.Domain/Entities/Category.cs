namespace DevSkill.Inventory.Domain.Entities
{
    public class Category : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public List<Product> Products { get; set; }
    }
}