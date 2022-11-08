namespace Guitaria.Data.Constants
{
    public static class ConstantValues
    {
        public static class Product
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 5000;

            public const int MinQuantity = 1;
            public const int MaxQuantity = 100;

            public const string PriceMinValue = "0.00";
            public const string PriceMaxValue = "100000.00";
        }
    }
}
