using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.MeasurementUnits.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MeasurementUnits.Queries
{
    public class MeasurementUnitGetQuery : IRequest<IList<MeasurementUnit>>, IMeasurementUnitGetQuery
    {
    }
}
