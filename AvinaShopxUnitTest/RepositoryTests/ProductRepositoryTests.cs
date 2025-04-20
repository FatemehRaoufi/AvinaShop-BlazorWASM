using AvinaShop.Data;
using AvinaShop.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;


namespace AvinaShop.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnv;

        public ProductRepositoryTests()
        {
            // Initialize in-memory database for isolated testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock IWebHostEnvironment to fake the web root path
            _mockWebHostEnv = new Mock<IWebHostEnvironment>();
            _mockWebHostEnv.Setup(env => env.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());

            // Initialize repository with mocks
            _repository = new ProductRepository(_context, _mockWebHostEnv.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Product()
        {
            // Arrange
            var product = new Product { Name = "Test", Price = 10, CategoryId = 1 };

            // Act
            var result = await _repository.CreateAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Single(_context.Product); // Ensure it was added to DB
        }

        [Fact]
        public async Task GetAsync_Should_Return_Correct_Product()
        {
            // Arrange
            var product = new Product { Name = "Test", Price = 20, CategoryId = 2 };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task GetAsync_Should_Return_New_When_Not_Found()
        {
            // Act
            var result = await _repository.GetAsync(999);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id); // Default Product object returned
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_GetAllProducts")
                .Options;

            var context = new ApplicationDbContext(options);

            // Add related category (if required)
            context.Category.Add(new Category { Id = 1, Name = "Category 1" });
            await context.SaveChangesAsync();

            // Add products
            context.Product.AddRange(
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 }
            );
            await context.SaveChangesAsync();

            var webHostEnvMock = new Mock<IWebHostEnvironment>();
            webHostEnvMock.Setup(env => env.WebRootPath).Returns(Directory.GetCurrentDirectory());

            var repository = new ProductRepository(context, webHostEnvMock.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }



        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Product()
        {
            // Arrange
            var product = new Product { Name = "Old", Price = 5, CategoryId = 1 };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Act
            product.Name = "Updated";
            var result = await _repository.UpdateAsync(product);

            // Assert
            Assert.Equal("Updated", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Product_And_Image()
        {
            // Arrange: Create a fake image file
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "test.jpg");
            File.WriteAllText(imagePath, "dummy image");

            var product = new Product
            {
                Name = "ToDelete",
                Price = 30,
                CategoryId = 1,
                ImageUrl = "/test.jpg"
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Act: Try deleting the product
            var result = await _repository.DeleteAsync(product.Id);

            // Assert: Check DB and file system
            Assert.True(result);
            Assert.Empty(_context.Product);
            Assert.False(File.Exists(imagePath)); // Image should be deleted
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnFalse_IfNotFound()
        {
            // Act
            var result = await _repository.DeleteAsync(9999);

            // Assert
            Assert.False(result); // Should return false for non-existing product
        }
    }
}
