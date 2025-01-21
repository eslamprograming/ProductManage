using Moq;
using Xunit;
using DAL.Entities;
using DAL.Repo;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Product.Tests
{
    public class ProductRepoTests
    {
        private readonly ProductRepo _productRepo;
        private readonly ApplicationDBContext _context;

        public ProductRepoTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDBContext(options);
            _productRepo = new ProductRepo(_context);
        }
        [Fact]
        public async Task AddProductAsync_ShouldAddProductSuccessfully()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Test Product",
                StartDate = DateTime.Now,
                Duration = 10,
                Price = 100,
                CategoryId = 1
            };

            // Act
            var response = await _productRepo.AddProductAsync(newProduct);

            // Assert
            Assert.True(response.success);
            Assert.Null(response.message);
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnErrorWhenExceptionOccurs()
        {
            // Arrange
            Product nullProduct = null;

            // Act
            var response = await _productRepo.AddProductAsync(nullProduct);

            // Assert
            Assert.False(response.success);
            Assert.Equal("Object reference not set to an instance of an object.", response.message);
        }
        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProductSuccessfully()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Product to Delete",
                StartDate = DateTime.Now,
                Duration = 10,
                Price = 150,
                CategoryId = 1
            };

            await _productRepo.AddProductAsync(newProduct);

            // Act
            var response = await _productRepo.DeleteProductAsync(newProduct.ProductId);

            // Assert
            Assert.True(response.success);
            Assert.Null(response.message);
        }
        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProductSuccessfully()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Old Product",
                StartDate = DateTime.Now,
                Duration = 10,
                Price = 200,
                CategoryId = 1
            };

            await _productRepo.AddProductAsync(newProduct);

            // Act
            var updatedProduct = new Product
            {
                Name = "Updated Product",
                StartDate = DateTime.Now,
                Duration = 20,
                Price = 250,
                CategoryId = 1
            };

            var response = await _productRepo.UpdateProductAsync("user1", newProduct.ProductId, updatedProduct);

            // Assert
            Assert.True(response.success);
            Assert.Equal("Updated Product", response.Value.Name);
            Assert.Equal(20, response.Value.Duration);
        }

    }
}
