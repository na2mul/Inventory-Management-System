using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, ITransactionalUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        public IProductRepository ProductRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IMeasurementUnitRepository MeasurementUnitRepository { get; set; }
        public ICustomerRepository CustomerRepository { get; set; }
        public ISaleRepository SaleRepository { get; set; }
        public IAccountTypeRepository AccountTypeRepository { get; set; }
        public IAccountRepository AccountRepository { get; set; }
        public ITransferAccountRepository TransferAccountRepository { get; set; }

        public ApplicationUnitOfWork(
            ApplicationDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMeasurementUnitRepository measurementUnitRepository,
            ICustomerRepository customerRepository,
            ISaleRepository saleRepository,
            IAccountTypeRepository accountTypeRepository,
            IAccountRepository accountRepository,
            ITransferAccountRepository transferAccountRepository
            ) : base(context)
        {
            _dbContext = context;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            MeasurementUnitRepository = measurementUnitRepository;
            CustomerRepository = customerRepository;
            SaleRepository = saleRepository;
            AccountTypeRepository = accountTypeRepository;
            AccountRepository = accountRepository;
            TransferAccountRepository = transferAccountRepository;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
    }
}
