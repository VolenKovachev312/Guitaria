using Guitaria.Data.Models;
using Guitaria.Models.Category;
using Guitaria.Models.Product;
namespace Guitaria.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductCardViewModel>> GetAllAsync(string categoryName);

        Task AddProductAsync(CreateProductViewModel model);

        Task RemoveProductAsync(RemoveProductViewModel model);

        Task<IEnumerable<Category>> LoadCategoriesAsync();

        Task<ProductViewModel> GetProductAsync(string productName);

        Task EditProductAsync(ProductViewModel model, string productName);
        Task<IEnumerable<Product>> LoadProductsAsync();

        Task AddProductToCartAsync(string userId, string productName);
    }
}
