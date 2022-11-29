using Guitaria.Data.Models;
using Guitaria.Models.Cart;

namespace Guitaria.Contracts
{
    public interface IShoppingCartService
    {
        Task<Dictionary<Product, int>> LoadProductsAsync(string userId);

        Task RemoveProductAsync(string userId,Guid productId);

        Task ClearCartAsync(string userId);
    }
}
