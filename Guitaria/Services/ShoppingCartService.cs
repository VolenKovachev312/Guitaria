using Guitaria.Contracts;
using Guitaria.Data;
using Guitaria.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private HttpContext? httpContext;
        private ITempDataDictionary tempData;

        public ShoppingCartService(ApplicationDbContext _context, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            httpContext = _httpContextAccessor.HttpContext;
            tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
        }
        public async Task AddOrderToHistoryAsync(string userId)
        {
            var user = await context.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc=>sc.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var purchaseHistory = user.PurchaseHistory;
            if (user.ShoppingCart.ShoppingCartProducts.Select(p=>p.Product).Count() == 0)
            {
                tempData["Error"]="Shopping cart is empty!";
                return;
            }
            var order = new Order();
            var finalPrice = user.ShoppingCart.ShoppingCartProducts.Select(p => p.Product).Sum(p => p.Price);
            await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [Orders](Id,PurchaseHistoryId,OrderDate,FinalPrice) VALUES ({order.Id},{user.PurchaseHistory.Id},{DateTime.Now},{finalPrice})");
            foreach (var product in user.ShoppingCart.ShoppingCartProducts)
            {
                await context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO [OrderProduct](OrderId,ProductId) VALUES ({order.Id},{product.ProductId})");
            }
            tempData["Checkout"] = "Order has been confirmed!";
            await context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            user?.ShoppingCart.ShoppingCartProducts.Clear();
            tempData["Success"] = "Shopping cart has been emptied.";
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

        public async Task<IEnumerable<Order>> LoadPurchaseHistoryAsync(string userId)
        {
            var user = await context.Users.Include(u => u.PurchaseHistory).ThenInclude(sc => sc.PurchasedProducts).ThenInclude(sc => sc.OrderProducts).ThenInclude(p=>p.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return user.PurchaseHistory.PurchasedProducts;
        }

        public async Task RemoveProductAsync(string userId, Guid productId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(p=>p.Product).FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            var shoppingCart = user?.ShoppingCart;
            var product = shoppingCart.ShoppingCartProducts.Select(p=>p.Product).FirstOrDefault(p => p.Id == productId);

            var shoppingCartProduct = shoppingCart.ShoppingCartProducts.FirstOrDefault(p => p.Product == product);
            shoppingCart.ShoppingCartProducts.Remove(shoppingCartProduct);
            tempData["Success"] = "Removed item from shopping cart.";
            await context.SaveChangesAsync();
        }
    }
}
