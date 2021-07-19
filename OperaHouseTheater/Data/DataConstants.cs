﻿namespace OperaHouseTheater.Data
{
    public class DataConstants
    {
        public class Admin 
        {
            public const int NameMaxLength = 25;
        }

        public class Employee 
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 3;
            public const int BiographyMinLength = 20;
            public const int BiographyMaxLength = 200;
        }

        public class News 
        {
            public const int NewsTitleMaxLength = 100;
            public const int NewsTitleMinLength = 10;
            public const int NewsContentMinLength = 100;
        }

        public class Performance
        {
            public const int TitleMaxLength = 30;
            public const int TitleMinLength = 3;
            public const int ContentMinLength = 100;
            public const int ComposerNameMaxLength = 30;
        }

        public const int RoleNameMaxLength = 30;




        public const int CategoryMaxLength = 50;

        public const int CommentMaxLength = 100;

        public const int MemberNameMaxLength = 30;


    }
}
