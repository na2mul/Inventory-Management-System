using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Features.Categories.Queries;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoryGetQuery : IRequest<IList<Category>>, ICategoryGetQuery
    {
    }
}
