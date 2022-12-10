using Guitaria.Contracts;
using Guitaria.Data.Models;
using Guitaria.Core.Models;
using Guitaria.Core.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Guitaria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public HomeController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            const string carouselProductsCacheKey = "CarouselProductsCacheKey";
            var carouselProducts = memoryCache.Get<IEnumerable<Product>>(carouselProductsCacheKey);
            if (carouselProducts == null)
            {
                carouselProducts = await productService.LoadCarouselAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                memoryCache.Set(carouselProductsCacheKey, carouselProducts, cacheOptions);
            }

            const string latestProductsCacheKey = "latestProductsCacheKey";
            var latestProducts = memoryCache.Get<IEnumerable<Product>>(latestProductsCacheKey);
            if (latestProducts == null)
            {
                latestProducts = await productService.LoadLatestAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                memoryCache.Set(latestProductsCacheKey, latestProducts, cacheOptions);
            }

            IndexViewModel model = new IndexViewModel()
            {
                CarouselProducts = carouselProducts,
                LatestProducts = latestProducts
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}