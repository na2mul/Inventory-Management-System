using System.Diagnostics.CodeAnalysis;
using Autofac.Extras.Moq;
using AutoMapper;
using DevSkill.Inventory.Application.Exceptions;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Moq;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests.Products
{
    [ExcludeFromCodeCoverage]
    public class ProductUpdateCommandHandlerTests
    {
        private AutoMock _moq;
        private ProductUpdateCommandHandler _handler;
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
            _handler = _moq.Create<ProductUpdateCommandHandler>();
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
        public async Task Handle_UniqueName_UpdatesProduct()
        {
            // Arrange
            var command = new ProductUpdateCommand
            {
                Id = Guid.NewGuid(),
                Name = "Test Product"
            };

            var product = new Product
            {
                Id = command.Id,
                Name = command.Name
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _mapperMock.Setup(x => x.Map<Product>(command))
                .Returns(product)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.IsNameDuplicate(product.Name, product.Id))
                .Returns(false)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.Update(product))                
                .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _productRepositoryMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }

        [Test]
        public async Task Handle_DuplicateName_ThrowsException()
        {
            // Arrange
            var command = new ProductUpdateCommand
            {
                Id = Guid.NewGuid(),
                Name = "Duplicate Product"
            };

            var product = new Product
            {
                Id = command.Id,
                Name = command.Name
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.ProductRepository)
                .Returns(_productRepositoryMock.Object);

            _mapperMock.Setup(x => x.Map<Product>(command))
                .Returns(product)
                .Verifiable();

            _productRepositoryMock.Setup(x => x.IsNameDuplicate(product.Name, product.Id))
                .Returns(true)
                .Verifiable();

            // Act & Assert
            await Should.ThrowAsync<DuplicateProductNameException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            // Verify that UpdateAsync and SaveAsync were never called
            _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            _applicationUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}