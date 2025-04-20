using Microsoft.EntityFrameworkCore;
using AvinaShop.Data;
using AvinaShop.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvinaShop.Repository
{
    // Repository class for handling category-related data operations
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        // Constructor to inject the database context
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Creates a new category in the database
        public async Task<Category> CreateAsync(Category category)
        {
            // Add the category to the DbContext and save changes
            await _db.Category.AddAsync(category);
            await _db.SaveChangesAsync();
            return category; // Return the created category
        }

        // Deletes a category by its ID
        public async Task<bool> DeleteAsync(int id)
        {
            // Find the category by ID
            var category = await _db.Category.FirstOrDefaultAsync(c => c.Id == id);

            // If the category exists, remove it and save changes
            if (category != null)
            {
                _db.Category.Remove(category);
                return (await _db.SaveChangesAsync()) > 0; // Return true if deletion was successful
            }

            // Return false if no category was found to delete
            return false;
        }

        // Retrieves a category by its ID
        public async Task<Category> GetAsync(int id)
        {
            // Find the category by ID
            var category = await _db.Category.FirstOrDefaultAsync(c => c.Id == id);

            // If no category found, return a new Category object
            return category ?? new Category();
        }

        // Retrieves all categories from the database
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Return all categories from the database
            return await _db.Category.ToListAsync();
        }

        // Updates an existing category in the database
        public async Task<Category> UpdateAsync(Category category)
        {
            // Find the category by ID
            var existingCategory = await _db.Category.FirstOrDefaultAsync(c => c.Id == category.Id);

            // If the category exists, update its properties
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;

                // Mark the category as updated and save changes
                _db.Category.Update(existingCategory);
                await _db.SaveChangesAsync();
                return existingCategory; // Return the updated category
            }

            // If the category doesn't exist, return the original category object
            return category;
        }
    }
}
