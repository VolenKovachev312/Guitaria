namespace Guitaria.Models.Cart
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<Data.Models.Product> Products { get; set; }=
        new List<Data.Models.Product>();
    }
}
