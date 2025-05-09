using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IAuthorRepository AuthorRepository { get; }
        public IBookRepository BookRepository { get; }
    }
}
