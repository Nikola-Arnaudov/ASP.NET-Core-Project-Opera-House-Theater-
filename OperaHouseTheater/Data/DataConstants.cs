namespace OperaHouseTheater.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMaxLength = 50;
            public const int FullNameMinLength = 5;
            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;
        }

        public class Admin 
        {
            public const int NameMaxLength = 25;
        }

        public class Employee 
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 2;
            public const int BiographyMinLength = 20;
            public const int BiographyMaxLength = 700;
        }

        public class News 
        {
            public const int NewsTitleMaxLength = 100;
            public const int NewsTitleMinLength = 10;
            public const int NewsContentMinLength = 100;
        }

        public class Performance
        {
            public const int TitleMaxLength = 50;
            public const int TitleMinLength = 2;
            public const int ContentMinLength = 100;
            public const int SynopsisMinLength = 100;
            public const int SynopsisMaxLength = 5000;
            public const int ComposerNameMaxLength = 30;
            public const int ComposerNameMinLength = 4;
        }

        public const int RoleNameMaxLength = 30;
        public const int RoleNameMinLength = 2;

        public const int CategoryMaxLength = 50;

        public const int CommentMaxLength = 300;
        public const int CommentMinLength = 4;

        public const int MemberNameMaxLength = 30;
        public const int MemberNameMinLength = 2;
        public const int PhoneNumberMaxLength = 20;
        public const int PhoneNumberMinLength = 5;

        public const int DefaultFreeSeats = 500;
    }
}
