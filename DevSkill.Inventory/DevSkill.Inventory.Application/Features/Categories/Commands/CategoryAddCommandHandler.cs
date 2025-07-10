using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Commands
{
    public class CategoryAddCommandHandler : IRequestHandler<CategoryAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public CategoryAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(CategoryAddCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            if (!_applicationUnitOfWork.CategoryRepository.IsNameDuplicate(category.CategoryName, category.Id))
            {
                await _applicationUnitOfWork.CategoryRepository.AddAsync(category);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateProductNameException();
        }
    }
}
