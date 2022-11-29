namespace Guitaria.Models.Cart
{
    public class ShoppingCartViewModel
    {
        public IDictionary<Data.Models.Product, int> Products { get; set; }=
        new Dictionary<Data.Models.Product,int>();
    }
}
