using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using KBS2.WijkagentApp.DataModels;

namespace KBS2.WijkagentApp.Assets
{
    class Constants
    {
        // mockupdata

        public static readonly Officer User = new Officer
        {
            OfficerId = "dae61b02-df6a-4173-b7bc-0bcecd9f9b2f",
            UserName = "Agent1",
            Password = "1234"
        };

        public static Person PoliceOfficer = new Person
        {
            PersonId = User.OfficerId,
            BirthDate = new DateTime(1950 - 01 - 01),
            FirstName = "Oom",
            LastName = "Agent",
            Description = "Wijkagent"
        };

        public static Person PoliceOfficer01 = new Person
        {
            PersonId = "fa336ca2-753b-4d17-875b-301ebc42ff18",
            Description = "Agent"   
        };

        public static ObservableCollection<Person> Persons = new ObservableCollection<Person>
        {
            new Person
            {
                PersonId = "08f06840-8107-4bd3-97aa-1455eeadacfb",
                FirstName = "Karen",
                LastName = "Bosch",
                Description = "Verdachte"
            },
            new Person
            {
                PersonId = "fd7f5586-4246-4774-8d64-14956c23cd26",
                FirstName = "Sake",
                LastName = "Elfring",
                Description = "Slachtoffer"
            },
            new Person
            {
                PersonId = "f595ec57-2299-4f8c-b0d9-19cd0db00d93",
                FirstName = "Joost",
                LastName = "Reijmer",
                Description = "Slachtoffer"
            },
            new Person
            {
                PersonId = "fa336ca2-753b-4d17-875b-301ebc42ff18",
                FirstName = "Ronald",
                LastName = "van Bostelen",
                Description = "Slachtoffer"
            }
        };

        public static ObservableCollection<Report> Reports = new ObservableCollection<Report>
        {
            new Report
            {
                ReportId = "d3fdcd26-553e-4f4d-9f3f-4df2d34fae8a",
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
                ReportId = "07b888e6-784f-43d5-a2e6-714db3a99471",
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
                ReportId = "4e3d9a10-ac81-40a6-8173-83dc581c28aa",
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
                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
            ReportId = Reports[0].ReportId,
            PersonId = Persons[3].PersonId, //Ronald
            Statement = "Ze viel mij zomaar aan",
            Type = 'V'
        },
            new ReportDetails
            {
                ReportId = Reports[1].ReportId,
                PersonId = Persons[0].PersonId, //Karen
                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[1].ReportId,
                PersonId = Persons[2].PersonId, //Joost
                Statement = "Ze viel mij zomaar aan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[2].ReportId,
                PersonId = Persons[0].PersonId, //Karen
                Statement = "Ik heb het niet gedaan",
                Type = 'V'
            },
            new ReportDetails
            {
                ReportId = Reports[2].ReportId,
                PersonId = Persons[3].PersonId, //Sake
                Description = "Persoon vermoord",
                Type = 'A'
            },
        };
    }
}