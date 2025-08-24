using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MeasurementUnits.Queries
{
    public class MeasurementUnitGetQueryHandler : IRequestHandler<MeasurementUnitGetQuery, IList<MeasurementUnit>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public MeasurementUnitGetQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IList<MeasurementUnit>> Handle(MeasurementUnitGetQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.MeasurementUnitRepository.GetOrderedMeasurementUnitsAsync();
        }
    }
}
