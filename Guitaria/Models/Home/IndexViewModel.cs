namespace Guitaria.Models.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Guitaria.Data.Models.Product> CarouselProducts { get; set; }

        public IEnumerable<Guitaria.Data.Models.Product> LatestProducts { get; set; }
    }
}
