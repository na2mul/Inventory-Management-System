using DevSkill.Inventory.Domain.Features.MeasurementUnits.Commands;
using MediatR;
public class EnsureMeasurementUnitExistsCommand : IRequest<Guid>, IEnsureMeasurementUnitExistsCommand
{
    public string? MeasurementUnitId { get; set; }

    public EnsureMeasurementUnitExistsCommand(string measurementUnitId)
    {
        MeasurementUnitId = measurementUnitId;
    }
}