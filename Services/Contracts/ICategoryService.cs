using Guitaria.Data.Models;
using Guitaria.Core.Models.Category;

namespace Guitaria.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();

        Task AddCategoryAsync(CreateCategoryViewModel model);

        Task RemoveCategoryAsync(RemoveCategoryViewModel model);

        Task<IEnumerable<Category>> LoadCategoriesAsync();

        Task<CategoryViewModel> GetCategoryAsync(string categoryName);

        Task EditCategoryAsync(CategoryViewModel model, string categoryName);
    }
}
