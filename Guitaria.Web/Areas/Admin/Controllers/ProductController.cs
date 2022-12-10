using Guitaria.Contracts;
using Guitaria.Core.Models.Product;
using Guitaria.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Guitaria.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly IProductService productService;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        public ProductController(IProductService productService, RoleManager<IdentityRole<Guid>> roleManager):base(roleManager)
        {
            this.productService = productService;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            CreateProductViewModel model = new CreateProductViewModel()
            {
                Categories = await productService.LoadCategoriesAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
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
            return RedirectToAction("All", "Product", new { area = "" });
        }
        [HttpGet]
        public async Task<IActionResult> UnlistProduct()
        {
            UnlistProductViewModel model = new UnlistProductViewModel()
            {
                Products = await productService.LoadProductsAsync()
            };
            return View(model);
        }

        [HttpPost]
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
                TempData["Error"] = ae.Message;
                return View(model);
            }
            model.Products = await productService.LoadProductsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string productName)
        {
            ProductViewModel model = new ProductViewModel();
            try
            {
                model = await productService.GetProductAsync(productName);
            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return RedirectToAction("All","Product", new { area = "" });
            }

            return View(model);
        }

        [HttpPost]
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
            return RedirectToAction("All","Product", new { area = "" });
        }
    }
}
