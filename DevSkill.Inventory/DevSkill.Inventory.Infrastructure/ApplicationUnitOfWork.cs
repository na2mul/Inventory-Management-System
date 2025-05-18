using DevSkill.Inventory.Domain;
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
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUnitOfWork(ApplicationDbContext context, IProductRepository productRepository)
            : base(context)
        {
            _dbContext = context;
            ProductRepository = productRepository;
        }

       
    }
}
