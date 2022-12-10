namespace Guitaria.Core.Models.Home
{
    using Guitaria.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<Product> CarouselProducts { get; set; }

        public IEnumerable<Product> LatestProducts { get; set; }
    }
}
