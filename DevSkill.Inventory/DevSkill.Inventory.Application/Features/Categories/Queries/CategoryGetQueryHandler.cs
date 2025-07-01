using DevSkill.Inventory.Application.Features.Products.Queries;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoryGetQueryHandler : IRequestHandler<CategoryGetQuery, IList<Category>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CategoryGetQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IList<Category>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetOrderedCategoriesAsync();
        }
    }
}
