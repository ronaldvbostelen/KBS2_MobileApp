using System;
using System.Collections.ObjectModel;
using KBS2.WijkagentApp.DataModels.old;

namespace KBS2.WijkagentApp.Assets
{
    class Constants
    {
        // mockupdata
//
//        public static readonly Officer User = new Officer
//        {
//            OfficerId = Guid.NewGuid(),
//            UserName = "Agent1",
//            Password = "1234"
//        };

        public static Person PoliceOfficer = new Person
        {
            PersonId = Guid.NewGuid(),
            BirthDate = new DateTime(1950 - 01 - 01),
            FirstName = "Oom",
            LastName = "Agent",
            Description = "Wijkagent"
        };

        public static Person PoliceOfficer01 = new Person
        {
            PersonId = Guid.NewGuid(),
            Description = "Agent"   
        };

        public static ObservableCollection<Person> Persons = new ObservableCollection<Person>
        {
            new Person
            {
                BirthDate = new DateTime(1985,05,15),
                PersonId = Guid.NewGuid(),
                FirstName = "Karen",
                LastName = "Bosch",
                Description = "Verdachte",
                Gender = "V"
            },
            new Person
            {
                BirthDate = new DateTime(1985,05,15),
                PersonId = Guid.NewGuid(),
                FirstName = "Sake",
                LastName = "Elfring",
                Description = "Slachtoffer",
                Gender = "M"
            },
            new Person
            {
                BirthDate = new DateTime(1985,05,15),
                PersonId = Guid.NewGuid(),
                FirstName = "Joost",
                LastName = "Reijmer",
                Description = "Slachtoffer",
                Gender = "M"
            },
            new Person
            {
                BirthDate = new DateTime(1985,05,15),
                PersonId = Guid.NewGuid(),
                FirstName = "Ronald",
                LastName = "van Bostelen",
                Description = "Slachtoffer",
                Gender = "M"
            }
        };

        public static ObservableCollection<Report> Reports = new ObservableCollection<Report>
        {
            new Report
            {
                ReportId = Guid.NewGuid(),
                ReporterId = PoliceOfficer01.PersonId,
                Type = "Zware mishandeling",
                Time = DateTime.Now.TimeOfDay,
                Location = "Zwolle CS",
                Status = 'A',
                Priority = 3,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Longitude = 6.090399,
                Latitude = 52.505969
            },
            new Report
            {
                ReportId = Guid.NewGuid(),
                ReporterId = PoliceOfficer01.PersonId,
                Type = "Poging tot doodslag",
                Time = DateTime.Now.TimeOfDay,
                Location = "GGD",
                Status = 'A',
                Priority = 2,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Longitude = 6.093015,
                Latitude = 52.508171
            },
            new Report
            {
                ReportId = Guid.NewGuid(),
                ReporterId = PoliceOfficer01.PersonId,
                Type = "Moord",
                Time = DateTime.Now.TimeOfDay,
                Location = "Wezenlanden park",
                Status = 'A',
                Priority = 1,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Longitude = 6.105814,
                Latitude = 52.507746
            },
        };

        public static ObservableCollection<ReportDetails> ReportDetails = new ObservableCollection<ReportDetails>
        {
            new ReportDetails
            {
                ReportId = Reports[0].ReportId,
                PersonId = Persons[0].PersonId, //Karen
                IsHeard = true,
//                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
            ReportId = Reports[0].ReportId,
            PersonId = Persons[3].PersonId, //Ronald
                IsHeard = false,
//            Statement = "Ze viel mij zomaar aan",
            Type = 'V'
        },
            new ReportDetails
            {
                ReportId = Reports[1].ReportId,
                PersonId = Persons[0].PersonId, //Karen
                IsHeard = false,
//                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[1].ReportId,
                PersonId = Persons[2].PersonId, //Joost
                IsHeard = false,
//                Statement = "Ze viel mij zomaar aan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[2].ReportId,
                PersonId = Persons[0].PersonId, //Karen
                IsHeard = false,
//                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[2].ReportId,
                PersonId = Persons[1].PersonId, //Sake
                IsHeard = false,
//                Description = "Persoon vermoord",
                Type = 'A'
            },
        };

        public static ObservableCollection<OfficialReport> OfficialReports = new ObservableCollection<OfficialReport>();
    }
}