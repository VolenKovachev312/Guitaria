using Guitaria.Contracts;
using Guitaria.Data.Models;
using Guitaria.Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guitaria.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(string productName)
        {
                var userId = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                await productService.AddProductToCartAsync(userId, productName);
            }
            catch(ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return RedirectToAction("ViewProduct", new { productName = productName });
            }
            TempData["Success"] = "Item added to cart successfully.";
            return RedirectToAction("ViewProduct", new { productName = productName });
        }

        public async Task<IActionResult> All(string categoryName, string searchQuery,int currentPage)
        {
            if (string.IsNullOrEmpty(categoryName) && string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("All", "Category");
            }

            var model = await productService.GetAllAsync(categoryName,searchQuery,currentPage);
            return View(model);
        }

        public async Task<IActionResult> ViewProduct(string productName)
        {
            ProductViewModel model;
            try
            {
                 model = await productService.GetProductAsync(productName);
            }
            catch(Exception ae)
            {
                TempData["Error"] = ae.Message;
                return RedirectToAction("All", "Category");
            }
            return View(model);
        }

    }
}
