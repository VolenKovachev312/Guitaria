using Guitaria.Contracts;
using Guitaria.Core.Models.Cart;
using Guitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        public async Task<IActionResult> PurchaseHistory()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            PurchaseHistoryViewModel model = new PurchaseHistoryViewModel()
            {
                Orders = await cartService.LoadPurchaseHistoryAsync(userId)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var model = new CheckoutViewModel()
            {
                Products = await cartService.LoadProductsCheckoutAsync(userId)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            model.Products = await cartService.LoadProductsCheckoutAsync(userId);
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fill out the whole form!";
                return View(model);
            }
            try
            {
                await cartService.AddOrderToHistoryAsync(userId);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return View(model);
            }

            await cartService.ClearCartAsync(userId);

            TempData["Checkout"] = "Order has been confirmed!";

            return RedirectToAction("Checkout");
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

            return RedirectToAction("ShowCart", "ShoppingCart");
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
