using System;

namespace KBS2.WijkagentApp.DataModels
{
    public static class User
    {
        public static Officer Base { get; set; }
        public static Person Person { get; set; }

        public static Guid Id => Base.OfficerId;
        public static Guid? PersonId => Base.PersonId;
        public static string Name => Base.UserName;
    }
}