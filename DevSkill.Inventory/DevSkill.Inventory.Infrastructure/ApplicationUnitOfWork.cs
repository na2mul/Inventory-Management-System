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
    public class ApplicationUnitOfWork : UnitOfWork, Domain.IApplicationUnitOfWork
    {
        public IAuthorRepository AuthorRepository { get; set; }

        public IBookRepository BookRepository { get; set; }
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUnitOfWork(ApplicationDbContext context, IAuthorRepository authorRepository,
            IBookRepository bookRepository) : base(context)
        {
            _dbContext = context;
            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
        }

       
    }
}
