using Guitaria.Data.Models;
using Guitaria.Models.Cart;

namespace Guitaria.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<Product>> LoadProductsAsync(string userId);

        Task RemoveProductAsync(string userId,Guid productId);
        Task ClearCartAsync(string userId);

        Task<ICollection<Product>> LoadProductsCheckoutAsync(string userId);

        Task AddOrderToHistoryAsync(string userId);

    }
}
