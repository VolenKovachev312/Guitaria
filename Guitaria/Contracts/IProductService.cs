using Guitaria.Data.Models;
using Guitaria.Models.Product;

namespace Guitaria.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductCardViewModel>> GetAllAsync();
        Task AddCategoryAsync(CreateCategoryViewModel model);

        Task RemoveCategoryAsync(RemoveCategoryViewModel model);

        Task<IEnumerable<Category>> LoadCategoriesAsync();

        Task AddProductAsync(CreateProductViewModel model);

        Task RemoveProductAsync(RemoveProductViewModel model);
    }
}
