using DAL.DBContext;
using DAL.Entities;
using DAL.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class RepoTest
    {
        private readonly ProductRepo _productRepo;
        private readonly ApplicationDBContext _context;
        public RepoTest()
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
                CreationDate= DateTime.Now,
                CreatedByUserId= "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                StartDate = DateTime.Now,
                Duration = 10,
                Price = 100,
                ImagePath="ImagePath",
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
            Assert.Contains("Value cannot be null", response.message);
        }
        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProductSuccessfully()
        {
            // Arrange
            var newProduct = new Product
            {
                Id=2,
                Name = "Product to Delete",
                CreationDate=DateTime.Now,
                StartDate = DateTime.Now,
                CreatedByUserId = "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                Duration = 10,
                Price = 150,
                ImagePath="ImagePath",
                CategoryId = 1
            };

            var result=await _productRepo.AddProductAsync(newProduct);

            // Act
            var response = await _productRepo.DeleteProductAsync(2);

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
                Id=2,
                Name = "Old Product",
                CreationDate=DateTime.Now,
                StartDate = DateTime.Now,
                CreatedByUserId = "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                Duration = 10,
                Price = 200,
                ImagePath="ImagePath",
                CategoryId = 1
            };

            await _productRepo.AddProductAsync(newProduct);

            // Act
            var updatedProduct = new Product
            {
                Name = "Updated Product",
                CreationDate=DateTime.Now,
                StartDate = DateTime.Now,
                CreatedByUserId = "84f67a6e-e54f-48bd-b949-cd3c754e4fb6",
                Duration = 20,
                Price = 250,
                ImagePath="ImagePath",
                CategoryId = 1
            };

            var response = await _productRepo.UpdateProductAsync("84f67a6e-e54f-48bd-b949-cd3c754e4fb6", 2, updatedProduct);

            // Assert
            Assert.True(response.success);
            Assert.Equal("Updated Product", response.Value.Name);
            Assert.Equal(20, response.Value.Duration);
        }

    }
}
