﻿using Guitaria.Data.Models;
using Guitaria.Core.Models.Cart;

namespace Guitaria.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<Order>> LoadPurchaseHistoryAsync(string userId);
        Task<IEnumerable<Product>> LoadProductsAsync(string userId);

        Task RemoveProductAsync(string userId,Guid productId);
        Task ClearCartAsync(string userId);

        Task<ICollection<Product>> LoadProductsCheckoutAsync(string userId);

        Task AddOrderToHistoryAsync(string userId);

    }
}
