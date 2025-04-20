using AvinaShop.Data;
using AvinaShop.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace AvinaShop.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryTests()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _categoryRepository = new CategoryRepository(_dbContext);
        }

        // Test CreateAsync method
        [Fact]
        public async Task CreateAsync_ShouldAddCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };

            // Act
            var result = await _categoryRepository.CreateAsync(category);

            // Assert
            Assert.NotNull(result); // Ensure the category is returned
            Assert.Equal("Test Category", result.Name); // Ensure the name is correct
            Assert.Contains(result, _dbContext.Category); // Ensure category is in the context
        }

        // Test DeleteAsync method
        [Fact]
        public async Task DeleteAsync_ShouldRemoveCategory()
        {
            // Arrange
            var category = new Category { Name = "Category to Delete" };
            await _categoryRepository.CreateAsync(category); // Add category first

            // Act
            var result = await _categoryRepository.DeleteAsync(category.Id);

            // Assert
            Assert.True(result); // Ensure the deletion was successful
            Assert.Null(await _dbContext.Category.FindAsync(category.Id)); // Ensure category no longer exists
        }

        // Test GetAsync method
        [Fact]
        public async Task GetAsync_ShouldReturnCategory()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            await _categoryRepository.CreateAsync(category); // Add category first

            // Act
            var result = await _categoryRepository.GetAsync(category.Id);

            // Assert
            Assert.NotNull(result); // Ensure the category is returned
            Assert.Equal("Test Category", result.Name); // Ensure the name is correct
        }

        // Test GetAllAsync method
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var category1 = new Category { Name = "Category 1" };
            var category2 = new Category { Name = "Category 2" };
            await _categoryRepository.CreateAsync(category1); // Add category 1
            await _categoryRepository.CreateAsync(category2); // Add category 2

            // Act
            var result = await _categoryRepository.GetAllAsync();

            // Assert
            Assert.NotEmpty(result); // Ensure categories are returned
            Assert.Contains(result, c => c.Name == "Category 1");
            Assert.Contains(result, c => c.Name == "Category 2");
        }

        // Test UpdateAsync method
        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category { Name = "Old Category" };
            await _categoryRepository.CreateAsync(category); // Add category first

            category.Name = "Updated Category"; // Modify category

            // Act
            var result = await _categoryRepository.UpdateAsync(category);

            // Assert
            Assert.NotNull(result); // Ensure category is returned
            Assert.Equal("Updated Category", result.Name); // Ensure the name is updated
        }

        // Test DeleteAsync when the category doesn't exist
        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenCategoryNotFound()
        {
            // Act
            var result = await _categoryRepository.DeleteAsync(999); // Pass non-existing ID

            // Assert
            Assert.False(result); // Ensure it returns false since the category doesn't exist
        }
    }
}
