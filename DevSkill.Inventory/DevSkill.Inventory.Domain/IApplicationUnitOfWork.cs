using DevSkill.Inventory.Domain.Repositories;

namespace DevSkill.Inventory.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IMeasurementUnitRepository MeasurementUnitRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public ISaleRepository SaleRepository { get; }
        public IAccountTypeRepository AccountTypeRepository { get; }
        public IAccountRepository AccountRepository { get; }
    }
}
