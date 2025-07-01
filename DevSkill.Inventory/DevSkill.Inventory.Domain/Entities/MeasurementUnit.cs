namespace DevSkill.Inventory.Domain.Entities
{
    public class MeasurementUnit : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}