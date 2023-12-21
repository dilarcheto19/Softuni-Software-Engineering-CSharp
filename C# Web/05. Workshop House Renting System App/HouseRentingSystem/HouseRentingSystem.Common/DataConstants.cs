﻿namespace HouseRentingSystem.Common
{
    public static class DataConstants
    {
        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class House
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 50;

            public const int AddressMinLength = 30;
            public const int AddressMaxLength = 150;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;

            public const int PricePerMonthMinValue = 0;
            public const int PricePerMonthMaxValue = 2000;
        }

        public static class Agent
        {
            public const int PhoneNumberMinLength = 7;
            public const int PhoneNumberMaxLength = 15;
        }

        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 1;
            public const int FirstNameMaxLength = 12;

            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 15;
        }

        public static class AdminUser
        {
            public const string AdminRole = "Administrator";
            public const string AdminEmail = "admin@mail.com";
        }
    }
}