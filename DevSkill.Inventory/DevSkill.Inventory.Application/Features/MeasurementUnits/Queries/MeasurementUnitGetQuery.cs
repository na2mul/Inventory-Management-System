using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MeasurementUnits.Queries
{
    public class MeasurementUnitGetQuery : IRequest<IList<MeasurementUnit>>
    {
    }
}
