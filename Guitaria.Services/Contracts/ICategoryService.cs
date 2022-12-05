using Guitaria.Data.Models;
using Guitaria.Models.Category;

namespace Guitaria.Services.Contracts
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
