using AutoMapper;
using DevSkill.Inventory.Application.Features.MeasurementUnits.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class EnsureMeasurementUnitExistsCommandHandler : IRequestHandler<EnsureMeasurementUnitExistsCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<EnsureMeasurementUnitExistsCommandHandler> _logger;

    public EnsureMeasurementUnitExistsCommandHandler(
        IMapper mapper,
        IMediator mediator,
        ILogger<EnsureMeasurementUnitExistsCommandHandler> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Guid> Handle(EnsureMeasurementUnitExistsCommand request, CancellationToken cancellationToken)
    {
        // If it's already a valid GUID, return it
        if (Guid.TryParse(request.MeasurementUnitId, out Guid existingMeasurementUnitId))
        {            
            return existingMeasurementUnitId;
        }

        var measurementUnitModel = new MeasurementUnit() { Name = request.MeasurementUnitId.Trim() };
        var measurementUnitAddCommand = _mapper.Map<MeasurementUnitAddCommand>(measurementUnitModel);
        measurementUnitAddCommand.Id = IdentityGenerator.NewSequentialGuid();

        await _mediator.Send(measurementUnitAddCommand, cancellationToken);

        _logger.LogInformation("Created measurement unit with ID: {MeasurementUnitId}", measurementUnitAddCommand.Id);
        return measurementUnitAddCommand.Id;
    }
}