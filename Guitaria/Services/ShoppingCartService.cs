using Guitaria.Contracts;
using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartService(ApplicationDbContext _context)
        {
            context= _context;
        }
        public async Task AddOrderToHistoryAsync(string userId)
        {
            var user = await context.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc=>sc.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var purchaseHistory = user.PurchaseHistory;
            var order = new Order();
            await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [Orders](Id,PurchaseHistoryId,OrderDate) VALUES ({order.Id},{user.PurchaseHistory.Id},{DateTime.Now})");
            foreach (var product in user.ShoppingCart.ShoppingCartProducts)
            {
                await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [OrderProduct](OrderId,ProductId) VALUES ({order.Id},{product.ProductId})");
            }
            await context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            user?.ShoppingCart.ShoppingCartProducts.Clear();
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc=>sc.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var shoppingCart = user.ShoppingCart;
            var items = shoppingCart.ShoppingCartProducts.Select(p => p.Product).ToList();
            return items.ToList();

        }

        public async Task<ICollection<Product>> LoadProductsCheckoutAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc=>sc.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var shoppingCart = user.ShoppingCart;

            return shoppingCart.ShoppingCartProducts.Select(p=>p.Product).ToList();
        }

        public async Task RemoveProductAsync(string userId, Guid productId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(p=>p.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            var shoppingCart = user?.ShoppingCart;
            var product = shoppingCart.ShoppingCartProducts.Select(p=>p.Product).FirstOrDefault(p => p.Id == productId);

            var shoppingCartProduct = shoppingCart.ShoppingCartProducts.FirstOrDefault(p => p.Product == product);
            shoppingCart.ShoppingCartProducts.Remove(shoppingCartProduct);
            await context.SaveChangesAsync();
        }
    }
}
