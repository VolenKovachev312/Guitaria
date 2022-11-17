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

        public static class Category
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;
        }

        public static class User
        {
            public const int UserNameMinLength = 5;
            public const int UserNameMaxLength = 50;

            public const int EmailMinLength = 10;
            public const int EmailMaxLength = 60;

            public const int PasswordMinLength = 5;
            public const int PasswordMaxLength = 20;
        }

        public static class SeedData
        {
            public const string AdministratorRole = "Administrator";
            public const string UserRole = "User";

            public const string AdminEmail = "admin@gmail.com";
            public const string AdminUserName = "AdminUserName";
            public const string AdminPassword = "Admin12#";
        }
    }
}
