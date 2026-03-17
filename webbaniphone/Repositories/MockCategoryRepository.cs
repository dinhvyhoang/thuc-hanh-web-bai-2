using webbaniphone.Models;

namespace webbaniphone.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        // Dùng static để dữ liệu được lưu lại trong suốt quá trình app chạy
        private static List<Category> _categoryList = new List<Category>
        {
            new Category { Id = 1, Name = "iPhone" },
            new Category { Id = 2, Name = "iPad" }
        };

        public async Task<IEnumerable<Category>> GetAllAsync() => await Task.FromResult(_categoryList);

        public async Task<Category> GetByIdAsync(int id) => await Task.FromResult(_categoryList.FirstOrDefault(c => c.Id == id));

        public async Task AddAsync(Category category)
        {
            _categoryList.Add(category);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Category category)
        {
            var index = _categoryList.FindIndex(c => c.Id == category.Id);
            if (index != -1) _categoryList[index] = category;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var category = _categoryList.FirstOrDefault(c => c.Id == id);
            if (category != null) _categoryList.Remove(category);
            await Task.CompletedTask;
        }
    }
}