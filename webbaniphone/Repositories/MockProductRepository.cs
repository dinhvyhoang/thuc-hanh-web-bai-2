using webbaniphone.Models;
using webbaniphone.Repositories;

namespace webbaniphone.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        // Thêm static để dữ liệu không bị reset khi chuyển trang
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "iPhone 15 Pro", Price = 25000000, Description = "Apple Intelligence", CategoryId = 1, ImageUrl = "/images/iphone15.jpg" }
        };

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_products);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task AddAsync(Product product)
        {
            // Tự động tăng ID nếu bạn chưa có DB thực
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1) _products[index] = product;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null) _products.Remove(product);
            await Task.CompletedTask;
        }
    }
}