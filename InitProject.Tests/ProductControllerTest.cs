using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using InitProject.Controllers;
using InitProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace InitProject.Tests
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _mockRepository = new Mock<IProductRepository>();
            _controller = new ProductController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetProducts_Should_Be_List()
        {
            int pageIndex = 1;
            int pageSize = 10;
            var expectedProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2},
                new Product { ProductId = 2, Name = "Name2", Description = "Description2", Price = 2, Quantity = 3},
            };
            // Arrage
            _mockRepository.Setup(e => e.GetPage(pageIndex, pageSize)).ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts(pageIndex, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value) as List<Product>;
            Assert.NotNull(okResult);
            Assert.NotNull(actualProducts);
            Assert.Equal(expectedProducts.Count, actualProducts.Count);
            Assert.Equal(expectedProducts[0].ProductId, actualProducts[0].ProductId);
            Assert.Equal(expectedProducts[0].Name, actualProducts[0].Name);
        }

        [Fact]
        public async Task GetProduct_Should_Be_Object()
        {
            var id = 1;
            var expectedProduct = new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };

            // Arrange
            _mockRepository.Setup(e => e.Find(It.IsAny<int>())).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            Assert.NotNull(result);
            var actualProduct = Assert.IsType<Product>(result.Value);
            Assert.Equal(expectedProduct.Name, actualProduct.Name);
        }

        [Fact]
        public async Task GetProduct_Should_Not_Found()
        {
            var id = 1;
            Product? nullItem = null;
            // Arrange
            _mockRepository.Setup(e => e.Find(It.IsAny<int>())).ReturnsAsync(nullItem);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PostProduct_Should_Be_Created()
        {
            var productDTO = new CreateProductDTO { Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };
            var expectedProduct = new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };
            // Arrange
            _mockRepository.Setup(e => e.Add(It.IsAny<Product>())).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.PostProduct(productDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var actualProduct = Assert.IsAssignableFrom<Product>(createdAtActionResult.Value);
            Assert.Equal(expectedProduct.ProductId, actualProduct.ProductId);
            Assert.Equal(expectedProduct.Name, actualProduct.Name);
        }

        [Fact]
        public async Task PutProduct_Should_Be_Object()
        {
            var id = 1;
            UpdateProductDTO productDTO = new UpdateProductDTO { Name = "Name2", Description = "Description2", Price = 2, Quantity = 3 };
            var prevProduct = new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };
            var expectedProduct = new Product { ProductId = 1, Name = "Name2", Description = "Description2", Price = 2, Quantity = 3 };

            //Arrange
            _mockRepository.Setup(e => e.Find(id)).ReturnsAsync(prevProduct);
            _mockRepository.Setup(e => e.Update(It.IsAny<Product>())).ReturnsAsync(expectedProduct);
            // Act
            var result = await _controller.PutProduct(id, productDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
            Assert.Equal(expectedProduct.ProductId, actualProduct.ProductId);
            Assert.Equal(expectedProduct.Name, actualProduct.Name);
        }

        [Fact]
        public async Task PutProduct_Should_Be_Not_Found()
        {
            var id = 1;
            UpdateProductDTO productDTO = new UpdateProductDTO { Name = "Name2", Description = "Description2", Price = 2, Quantity = 3 };
            Product nullItem = null;
            var prevProduct = new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };
            var expectedProduct = new Product { ProductId = 1, Name = "Name2", Description = "Description2", Price = 2, Quantity = 3 };
            //Arrange
            _mockRepository.Setup(e => e.Find(id)).ReturnsAsync(nullItem);
            _mockRepository.Setup(e => e.Update(expectedProduct)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.PutProduct(id, productDTO);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_Should_Be_204()
        {
            var id = 1;
            UpdateProductDTO productDTO = new UpdateProductDTO { Name = "Name2", Description = "Description2", Price = 2, Quantity = 3 };
            var prevProduct = new Product { ProductId = 1, Name = "Name1", Description = "Description1", Price = 1, Quantity = 2 };
            //Arrange
            _mockRepository.Setup(e => e.Find(id)).ReturnsAsync(prevProduct);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result.Result);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
