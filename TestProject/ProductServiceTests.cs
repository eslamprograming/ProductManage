using BLL.Service;
using DAL.Entities;
using DAL.Repo;
using DAL.ModelsVM.SheardModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using DAL.DBContext;
using Microsoft.AspNetCore.Http;
using Moq;

namespace TestProject
{
    public class ProductServiceTests
    {
        private readonly ProductRepo _productRepo;
        private readonly ProductService _productService;
        private readonly ApplicationDBContext _context;

        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDBContext(options);
            _productRepo = new ProductRepo(_context);
            _productService = new ProductService(_productRepo);
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnSuccessResponse_WhenProductIsAdded()
        {
            // Arrange
            var imageMock = new Mock<IFormFile>();
            var content = new byte[] { 0, 1, 2, 3 };  // محتوى وهمي للصورة
            var fileName = "test_image.jpg";
            var stream = new MemoryStream(content);

            imageMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            imageMock.Setup(_ => _.FileName).Returns(fileName);
            imageMock.Setup(_ => _.Length).Returns(content.Length);
            var userId = "123";
            var productVM = new DAL.ModelsVM.ProductVM.productVM
            {
                Name = "Test Product",
                Price = 100,
                StartDate = DateTime.Now,
                Duration = 10,
                CategoryId = 1,
                Image = imageMock.Object
            };

            var product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                CreationDate = DateTime.Now,
                CreatedByUserId = userId,
                ImagePath = "ImagePath",
                StartDate = productVM.StartDate,
                Duration = productVM.Duration,
                CategoryId = productVM.CategoryId
            };

            // Act
            var result = await _productService.AddProductAsync(userId, productVM);

            // Assert
            Assert.True(result.success);
            //Assert.NotNull(result.Value);
            //Assert.Equal(productVM.Name, result.Value.Name);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnSuccessResponse_WhenProductIsDeleted()
        {
            // Arrange
            var productId = 2;
            var product = new Product
            {
                Id = 2,
                Name = "Product to Delete",
                CreationDate = DateTime.Now,
                StartDate = DateTime.Now,
                CreatedByUserId = "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                Duration = 10,
                Price = 150,
                ImagePath = "ImagePath",
                CategoryId = 1
            };
            

            await _productRepo.AddProductAsync(product);

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            Assert.True(result.success);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 2;
            var product = new Product
            {
                Id = productId,
                Name = "Product to Delete",
                CreationDate = DateTime.Now,
                StartDate = DateTime.Now,
                CreatedByUserId = "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                Duration = 10,
                Price = 150,
                ImagePath = "ImagePath",
                CategoryId = 1
            };

            await _productRepo.AddProductAsync(product);

            // Act
            var result = await _productService.GetProductbyId(productId);

            // Assert
            Assert.True(result.success);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
        }
    }
}
