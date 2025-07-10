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

namespace DevSkill.Inventory.Application.Features.MeasurementUnits.Commands
{
    public class MeasurementUnitAddCommandHandler : IRequestHandler<MeasurementUnitAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public MeasurementUnitAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(MeasurementUnitAddCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<MeasurementUnit>(request);
            if (!_applicationUnitOfWork.MeasurementUnitRepository.IsNameDuplicate(product.Name, product.Id))
            {
                await _applicationUnitOfWork.MeasurementUnitRepository.AddAsync(product);
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new DuplicateProductNameException();
        }
    }
}
