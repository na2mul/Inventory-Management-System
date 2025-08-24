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
using DevSkill.Inventory.Infrastructure;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleAddCommandHandler : IRequestHandler<SaleAddCommand>
    {
        private readonly ITransactionalUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;
        public SaleAddCommandHandler(ITransactionalUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(SaleAddCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _applicationUnitOfWork.BeginTransactionAsync();

            try
            {
                var sale = _mapper.Map<Sale>(request);
                if (sale.SalesDetails == null || sale.SalesDetails.Count == 0)
                    throw new InvalidOperationException("At least one product must be selected.");

                
                await _applicationUnitOfWork.SaleRepository.AddAsync(sale);

                // Deduct product stocks
                foreach (var item in sale.SalesDetails)
                {
                    var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"Product {item.ProductId} not found.");

                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for {product.Name}.");

                    product.Stock -= item.Quantity;
                    _applicationUnitOfWork.ProductRepository.Update(product);
                }

                await _applicationUnitOfWork.SaveAsync();
                await _applicationUnitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _applicationUnitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
