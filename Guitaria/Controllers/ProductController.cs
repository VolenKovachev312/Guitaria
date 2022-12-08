using Guitaria.Contracts;
using Guitaria.Data.Models;
using Guitaria.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guitaria.Controllers
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

        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProduct()
        {
            CreateProductViewModel model = new CreateProductViewModel()
            {
                Categories = await productService.LoadCategoriesAsync()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProduct(CreateProductViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await productService.AddProductAsync(model);
            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                model.Categories = await productService.LoadCategoriesAsync();
                return View(model);
            }
            return RedirectToAction("All", "Product");
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UnlistProduct()
        {
            UnlistProductViewModel model = new UnlistProductViewModel()
            {
                Products = await productService.LoadProductsAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UnlistProduct(UnlistProductViewModel model)
        {
            model.Products = await productService.LoadProductsAsync();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await productService.UnlistProductAsync(model);
            }
            catch (ArgumentException ae)
            {
                TempData["Error"]=ae.Message;
                return View(model);
            }
            model.Products = await productService.LoadProductsAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string productName)
        {
            ProductViewModel model = new ProductViewModel();
            try
            {
                 model = await productService.GetProductAsync(productName);
            }
            catch(ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return RedirectToAction("All");
            }
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(ProductViewModel model, string productName)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await productService.EditProductAsync(model, productName);

            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return View(model);
            }
            return RedirectToAction("All");
        }
       

    }
}
