using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Exceptions;

namespace DevSkill.Inventory.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AuthorService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddAuthor(Author author)
        {
            if (!_applicationUnitOfWork.AuthorRepository.IsNameDuplicate(author.Name))
            {
                _applicationUnitOfWork.AuthorRepository.Add(author);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateAuthorNameException();
        }

        public void DeleteAuthor(Guid id)
        {
            _applicationUnitOfWork.AuthorRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Author GetAuthor(Guid id)
        {
            return _applicationUnitOfWork.AuthorRepository.GetById(id);
        }

        public (IList<Author> data, int total, int totalDisplay) GetAuthors(int pageIndex, int pageSize,
            string? order, DataTablesSearch search)
        {
           return _applicationUnitOfWork.AuthorRepository.GetPagedAuthors(pageIndex, pageSize, order, search);
        }

        public void Update(Author author)
        {
            if (!_applicationUnitOfWork.AuthorRepository.IsNameDuplicate(author.Name, author.Id))
            {
                _applicationUnitOfWork.AuthorRepository.Update(author);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateAuthorNameException();

        }
    }
}
