namespace Guitaria.Models.Cart
{
    public class PurchaseHistoryViewModel
    {
        public IEnumerable<Guitaria.Data.Models.Order> Orders { get; set; }
        =new List<Guitaria.Data.Models.Order>();

    }
}
