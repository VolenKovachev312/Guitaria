using Guitaria.Contracts;
using Guitaria.Models.Cart;
using Guitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guitaria.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService cartService;
        public ShoppingCartController(IShoppingCartService _cartService)
        {
            cartService = _cartService;
        }
        public async Task<IActionResult> ShowCart()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartViewModel model = new ShoppingCartViewModel()
            {
                Products = await cartService.LoadProductsAsync(userId)
            };

            return View(model);
        }

        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await cartService.RemoveProductAsync(userId, productId);

            TempData["Success"] = "Removed item from shopping cart.";
            return RedirectToAction("ShowCart","ShoppingCart");
        }

        public async Task<IActionResult> ClearCart()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await cartService.ClearCartAsync(userId);

            TempData["Success"] = "Shopping cart has been emptied.";
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
    }
}
