using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IMeasurementUnitRepository : IRepository<MeasurementUnit,Guid>
    {
        public Task<IList<MeasurementUnit>> GetOrderedMeasurementUnitsAsync();
        public bool IsNameDuplicate(string name, Guid? id = null);

    }
}
