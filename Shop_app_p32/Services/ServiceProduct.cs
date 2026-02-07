using Shop_app_p32.Models;

namespace Shop_app_p32.Services
{
    public interface IServiceProduct
    {
        public Task<Product?> CreateAsync(Product? product);
        public Task<Product?> UpdateAsync(int id, Product? product);
        public Task<Product?> DeleteAsync(int id);
        public Task<IEnumerable<Product?>> GetAsync();
        public Task<Product?> GetByIdAsync(int id);
    }
    public class ServiceProduct : IServiceProduct
    {
        private readonly ProductContext _context;
        public ServiceProduct(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product?> CreateAsync(Product? product)
        {
            if (product == null) return null;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            Product? target = await GetByIdAsync(id);
            if (target == null) return null;
            _context.Products.Remove(target);
            await _context.SaveChangesAsync();
            return target;
        }

        public async Task<IEnumerable<Product?>> GetAsync()
        {
            var products = _context.Products;
            if (products == null) throw new ArgumentNullException();
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product =  await _context.Products.FindAsync(id);
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product? product)
        {
            var product_update = await _context.Products.FindAsync(id);
            if (product_update == null) return null;
            product_update.Name = product?.Name;
            product_update.Description = product?.Description;
            product_update.Price = product.Price;
            await _context.SaveChangesAsync();
            return product_update;
        }
    }
}
