using System.Diagnostics.CodeAnalysis;
using Autofac.Extras.Moq;
using AutoMapper;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.Products
{
    [ExcludeFromCodeCoverage]
    public class ProductDamageCommandHandlerTests
    {
        private AutoMock _moq;
        private ProductDamageCommandHandler _handler;
        private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _moq = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _moq?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _handler = _moq.Create<ProductDamageCommandHandler>();
            _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
            _productRepositoryMock = _moq.Mock<IProductRepository>();
            _mapperMock = _moq.Mock<IMapper>();
        }

        [TearDown]
        public void Teardown()
        {
            _applicationUnitOfWorkMock?.Reset();
            _productRepositoryMock?.Reset();
            _mapperMock?.Reset();
        }

        [Test]
        public async Task Handle_ValidDamageStock_UpdatesProductStockAndDamageStock()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var damageStock = 5;
            var initialStock = 20;
            var initialDamageStock = 2;

            var command = new ProductDamageCommand
            {
                Id = productId,
                Stock = damageStock
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock,
                DamageStock = initialDamageStock
            };

            var updatedProductCopy = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock - damageStock, 
                DamageStock = initialDamageStock + damageStock 
            };

            var mappedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock - damageStock,
                DamageStock = initialDamageStock + damageStock
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct)
                .Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(It.Is<Product>(p =>
                p.Id == productId &&
                p.Stock == initialStock - damageStock &&
                p.DamageStock == initialDamageStock + damageStock)))
                .Returns(mappedProduct)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.Update(mappedProduct))
                .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingProduct.Stock.ShouldBe(initialStock - damageStock);
            existingProduct.DamageStock.ShouldBe(initialDamageStock + damageStock);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _productRepositoryMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }

        [Test]
        public async Task Handle_ZeroDamageStock_DoesNotChangeStocks()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var damageStock = 0;
            var initialStock = 20;
            var initialDamageStock = 2;

            var command = new ProductDamageCommand
            {
                Id = productId,
                Stock = damageStock
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock,
                DamageStock = initialDamageStock
            };

            var mappedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock,
                DamageStock = initialDamageStock
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct)
                .Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(It.Is<Product>(p =>
                p.Id == productId &&
                p.Stock == initialStock &&
                p.DamageStock == initialDamageStock)))
                .Returns(mappedProduct)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.Update(mappedProduct))
                .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingProduct.Stock.ShouldBe(initialStock);
            existingProduct.DamageStock.ShouldBe(initialDamageStock);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _productRepositoryMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }

        [Test]
        public async Task Handle_DamageStockExceedsAvailableStock_UpdatesStockToNegative()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var damageStock = 15;
            var initialStock = 10;
            var initialDamageStock = 5;

            var command = new ProductDamageCommand
            {
                Id = productId,
                Stock = damageStock
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock,
                DamageStock = initialDamageStock
            };

            var mappedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock - damageStock, // -5
                DamageStock = initialDamageStock + damageStock // 20
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct)
                .Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(It.Is<Product>(p =>
                p.Id == productId &&
                p.Stock == initialStock - damageStock &&
                p.DamageStock == initialDamageStock + damageStock)))
                .Returns(mappedProduct)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.Update(mappedProduct))
                .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingProduct.Stock.ShouldBe(-5);
            existingProduct.DamageStock.ShouldBe(20);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _productRepositoryMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }

        [Test]
        public async Task Handle_ProductNotFound_ThrowsException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new ProductDamageCommand
            {
                Id = productId,
                Stock = 5
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync((Product)null)
                .Verifiable();

            // Act & Assert
            await Should.ThrowAsync<NullReferenceException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            // Verify that Update and SaveAsync were never called
            _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            _applicationUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);
            _mapperMock.Verify(x => x.Map<Product>(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public async Task Handle_NullStock_UpdatesWithZero()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var initialStock = 20;
            var initialDamageStock = 2;

            var command = new ProductDamageCommand
            {
                Id = productId,
                Stock = null
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock,
                DamageStock = initialDamageStock
            };

            var mappedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Stock = initialStock, // No change as null is treated as 0
                DamageStock = initialDamageStock // No change as null is treated as 0
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct)
                .Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(It.IsAny<Product>()))
                .Returns(mappedProduct)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.Update(mappedProduct))
                .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert - Stock should remain unchanged when null stock is provided
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _productRepositoryMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }
    }
}