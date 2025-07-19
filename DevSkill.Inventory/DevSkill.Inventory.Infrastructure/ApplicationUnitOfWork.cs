using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public IMeasurementUnitRepository MeasurementUnitRepository { get; set; }
        public ICustomerRepository CustomerRepository { get; set; }
        public ISaleRepository SaleRepository { get; set; }
        public IAccountTypeRepository AccountTypeRepository { get; set; }
        public IAccountRepository AccountRepository { get; set; }

        private readonly ApplicationDbContext _dbContext;
        public ApplicationUnitOfWork(
            ApplicationDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMeasurementUnitRepository measurementUnitRepository,
            ICustomerRepository customerRepository,
            ISaleRepository saleRepository,
            IAccountTypeRepository accountTypeRepository,
            IAccountRepository accountRepository
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
        }
    }
}
