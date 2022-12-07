namespace Guitaria.Models.Product
{
    public class AllProductsViewModel
    {
        public const int ProductsPerPage = 4;

        public int CurrentPage { get; set; } = 1;

        public int NumberOfPages { get; set; }

        public string SearchQuery { get; set; }

        public IEnumerable<ProductCardViewModel> Products { get; set; }
    }
}
