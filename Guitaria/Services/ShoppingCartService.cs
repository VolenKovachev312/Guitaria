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

        public async Task ClearCartAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.Products).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            user?.ShoppingCart.Products.Clear();
            await context.SaveChangesAsync();
        }

        public async Task<Dictionary<Product, int>> LoadProductsAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.Products).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var shoppingCart = user.ShoppingCart;
            Dictionary<Product,int> items= new Dictionary<Product,int>();
            foreach(var item in shoppingCart.Products)
            {
                items.Add(item,item.Quantity);
            }
            return items;
               
        }

        public async Task<ICollection<Product>> LoadProductsCheckoutAsync(string userId)
        {
            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.Products).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var shoppingCart = user.ShoppingCart;

            return shoppingCart.Products.ToList();
        }

        public async Task RemoveProductAsync(string userId, Guid productId)
        {
            var user = await context.Users.Include(u=>u.ShoppingCart).ThenInclude(sc=>sc.Products).FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            var shoppingCart = user?.ShoppingCart;
            var product = shoppingCart.Products.FirstOrDefault(p => p.Id == productId);
            
            shoppingCart.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
