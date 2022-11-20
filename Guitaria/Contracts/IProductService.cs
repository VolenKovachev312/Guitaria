using Guitaria.Data.Models;
using Guitaria.Models.Product;

namespace Guitaria.Contracts
{
    public interface IProductService
    {
        Task AddCategoryAsync(CreateCategoryViewModel model);

        Task RemoveCategoryAsync(CreateCategoryViewModel model);

        Task<IEnumerable<Category>> LoadCategoriesAsync();

        Task AddProductAsync(CreateProductViewModel model);

        Task RemoveProductAsync(RemoveProductViewModel model);
    }
}
