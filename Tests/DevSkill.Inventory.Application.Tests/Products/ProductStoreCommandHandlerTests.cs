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
    public class ProductStoreCommandHandlerTests
    {
        private AutoMock _moq;
        private ProductStoreCommandHandler _handler;
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
            _handler = _moq.Create<ProductStoreCommandHandler>();
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
        public async Task Handle_ValidStock_AddsStockToProduct()
        {
            // Arrange
            var command = new ProductStoreCommand
            {
                Id = Guid.NewGuid(),
                Stock = 10
            };

            var existingProduct = new Product
            {
                Id = command.Id,
                Name = "Test Product",
                Stock = 5
            };

            var mappedProduct = new Product
            {
                Id = command.Id,
                Name = "Test Product",
                Stock = 15
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingProduct)
                .Verifiable();

            _mapperMock.Setup(x => x.Map<Product>(existingProduct))
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
            existingProduct.Stock.ShouldBe(15);

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
            var command = new ProductStoreCommand
            {
                Id = Guid.NewGuid(),
                Stock = 10
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Product)null)
                .Verifiable();

            // Act & Assert
            await Should.ThrowAsync<NullReferenceException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            // Verify that Update and SaveAsync were never called
            _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            _applicationUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}