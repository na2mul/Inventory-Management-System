using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

public class EnsureCategoryExistsCommandHandler : IRequestHandler<EnsureCategoryExistsCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<EnsureCategoryExistsCommandHandler> _logger;

    public EnsureCategoryExistsCommandHandler(
        IMapper mapper,
        IMediator mediator,
        ILogger<EnsureCategoryExistsCommandHandler> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Guid> Handle(EnsureCategoryExistsCommand request, CancellationToken cancellationToken)
    {
        // If it's already a valid GUID, return it
        if (Guid.TryParse(request.CategoryId, out Guid existingCategoryId))
        {            
            return existingCategoryId;
        }        

        var categoryModel = new Category() { CategoryName = request.CategoryId.Trim() };
        var categoryAddCommand = _mapper.Map<CategoryAddCommand>(categoryModel);
        categoryAddCommand.Id = IdentityGenerator.NewSequentialGuid();

        await _mediator.Send(categoryAddCommand, cancellationToken);

        _logger.LogInformation("Created category with ID: {CategoryId}", categoryAddCommand.Id);
        return categoryAddCommand.Id;
    }
}