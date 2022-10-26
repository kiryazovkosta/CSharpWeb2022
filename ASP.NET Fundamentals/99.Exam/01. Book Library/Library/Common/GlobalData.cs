namespace Library.Common
{
    public class GlobalData
    {
        public class Category
        {
            public const int MinNameLength = 5;
            public const int MaxNameLength = 50;
        }

        public class User
        {
            public const int MinUserNameLength = 5;
            public const int MaxUserNameLength = 20;

            public const int MinEmailLength = 10;
            public const int MaxEmailLength = 60;

            public const int MinPasswordLength = 5;
            public const int MaxPasswordLength = 20;
        }

        public class Book
        {
            public const int MinTitleLength = 10;
            public const int MaxTitleLength = 50;

            public const int MinAuthorLength = 5;
            public const int MaxAuthorLength = 50;

            public const int MinDescriptionLength = 5;
            public const int MaxDescriptionLength = 5000;

            public const string MinRatingLength = "0.00";
            public const string MaxRatingLength = "10.00";
        }

        public class ControllersActionsNames
        {
            public const string IndexAction = "Index";
            public const string AllAction = "All";

            public const string HomeControllerName = "Home";
            public const string BooksControllerName = "Books";
        }
    }
}
