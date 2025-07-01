using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoryGetQuery : IRequest<IList<Category>>
    {
    }
}
