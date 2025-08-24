using DevSkill.Inventory.Domain.Features.Categories.Commands;
using MediatR;

public class EnsureCategoryExistsCommand : IRequest<Guid>, IEnsureCategoryExistsCommand
{
    public string CategoryId { get; set; }

    public EnsureCategoryExistsCommand(string categoryId)
    {
        CategoryId = categoryId;
    }
}
