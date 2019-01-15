using System;
using System.Collections.Generic;
using System.Text;
using KBS2.WijkagentApp.DataModels;

namespace KBS2.WijkagentApp.Assets
{
    class Constants
    {
        public static readonly Officer User = new Officer
        {
            OfficerId = "B5BE876A47F7BDE5A03431E9B780366A718121C80881017456E40DB61B601DD1",
            UserName = "Agent1",
            Password = "1234"
        };

        public static Person PoliceOfficer = new Person
        {
            PersonId = Constants.User.OfficerId,
            BirthDate = new DateTime(1950-01-01),
            FirstName = "Oom",
            LastName = "Agent",
            Description = "Wijkagent"
        };
    }
}
