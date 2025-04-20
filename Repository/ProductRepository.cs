using AvinaShop.Data;
using AvinaShop.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AvinaShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor to initialize the dependencies: Database context and WebHost environment
        public ProductRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // Creates a new product in the database
        public async Task<Product> CreateAsync(Product product)
        {
            await _db.Product.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        // Deletes a product by its ID, and removes the associated image from the server
        public async Task<bool> DeleteAsync(int id)
        {
            // Find the product by ID
            var product = await _db.Product.FirstOrDefaultAsync(p => p.Id == id);

            // If product is found, delete the associated image from the server
            if (product != null)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath); // Delete the product image
                }

                // Remove the product from the database
                _db.Product.Remove(product);
                return await _db.SaveChangesAsync() > 0; // Return true if the product is deleted
            }

            // Return false if no product is found with the given ID
            return false;
        }

        // Retrieves a product by its ID
        public async Task<Product> GetAsync(int id)
        {
            var product = await _db.Product.FirstOrDefaultAsync(p => p.Id == id);

            // If no product is found, return a new product
            return product ?? new Product();
        }

        // Retrieves all products, including their related categories
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Product.Include(p => p.Category).ToListAsync();
        }

        // Updates an existing product in the database
        public async Task<Product> UpdateAsync(Product product)
        {
            // Find the product by ID
            var productFromDb = await _db.Product.FirstOrDefaultAsync(p => p.Id == product.Id);

            // If the product exists in the database, update its properties
            if (productFromDb != null)
            {
                productFromDb.Name = product.Name;
                productFromDb.Description = product.Description;
                productFromDb.ImageUrl = product.ImageUrl;
                productFromDb.CategoryId = product.CategoryId;
                productFromDb.Price = product.Price;

                // Update the product in the database
                _db.Product.Update(productFromDb);
                await _db.SaveChangesAsync();
                return productFromDb;
            }

            // Return the original product if no matching product is found
            return product;
        }
    }
}
