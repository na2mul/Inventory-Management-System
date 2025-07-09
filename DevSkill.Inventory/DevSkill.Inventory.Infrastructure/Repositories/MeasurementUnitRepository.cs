using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Products.Queries;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class MeasurementUnitRepository : Repository<MeasurementUnit, Guid>, IMeasurementUnitRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MeasurementUnitRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        
        public async Task<IList<MeasurementUnit>> GetOrderedMeasurementUnitsAsync()
        {
            return await GetAsync(null, x => x.OrderBy(y => y.Name), null, true);
        }
        
    }
}
