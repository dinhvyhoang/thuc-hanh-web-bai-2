using webbaniphone.Models;

namespace webbaniphone.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}
