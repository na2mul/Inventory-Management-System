using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public ProductService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddProduct(Product product)
        {
            if (!_applicationUnitOfWork.ProductRepository.IsNameDuplicate(product.Name))
            {
                _applicationUnitOfWork.ProductRepository.Add(product);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateNameException("Product");
        }

        public void DeleteProduct(Guid id)
        {
            _applicationUnitOfWork.ProductRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Product GetProduct(Guid id)
        {
            return _applicationUnitOfWork.ProductRepository.GetById(id);
        }

        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize,
            string? order, DataTablesSearch search)
        {
            return _applicationUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, order, search);
        }

        //public async Task<(IList<ProductListDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
        //    string? order, ProductSearchDto search)
        //{
        //    //return await _applicationUnitOfWork.GetProductsSP(pageIndex, pageSize, order, search);
        //}

        public void Update(Product product)
        {
            if (!_applicationUnitOfWork.ProductRepository.IsNameDuplicate(product.Name, product.Id))
            {
                _applicationUnitOfWork.ProductRepository.Update(product);
                _applicationUnitOfWork.Save();
            }
            else
                throw new DuplicateNameException("Product");

        }
    }
}
