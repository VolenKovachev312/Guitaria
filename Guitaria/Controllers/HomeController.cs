using Guitaria.Contracts;
using Guitaria.Models;
using Guitaria.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Guitaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel model = new IndexViewModel()
            {
                CarouselProducts = await productService.LoadCarouselAsync(),
                LatestProducts = await productService.LoadLatestAsync()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel model)
        {
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}