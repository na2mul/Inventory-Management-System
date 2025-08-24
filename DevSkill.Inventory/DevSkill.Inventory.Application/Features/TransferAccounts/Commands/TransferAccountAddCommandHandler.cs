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

namespace DevSkill.Inventory.Application.Features.TransferAccounts.Commands
{
    public class TransferAccountAddCommandHandler : IRequestHandler<TransferAccountAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public TransferAccountAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(TransferAccountAddCommand request, CancellationToken cancellationToken)
        {
            var transferAccount = _mapper.Map<TransferAccount>(request);            
            await _applicationUnitOfWork.TransferAccountRepository.AddAsync(transferAccount);
            await _applicationUnitOfWork.SaveAsync();        
        }
    }
}
