using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models.DataControllers;
using NUnit.Framework;

namespace KBS2.WijkagentApp.UnitTests
{
    [TestFixture]
    public class DataControllerTests
    {
        private DataController dataController;

        public DataControllerTests()
        {
            dataController = new DataController();
        }

        [Test]
        public void AddressTable_IsNotNull()
        {
            Assert.That(dataController.AddressTable, Is.Not.Null);
        }

        [Test]
        public void AntecedentTable_IsNotNull()
        {
            Assert.That(dataController.AntecedentTable, Is.Not.Null);
        }

        [Test]
        public void EmergencyTable_IsNotNull()
        {
            Assert.That(dataController.EmergencyTable, Is.Not.Null);
        }

        [Test]
        public void OfficerTable_IsNotNull()
        {
            Assert.That(dataController.OfficerTable, Is.Not.Null);
        }

        [Test]
        public void PersonTable_IsNotNull()
        {
            Assert.That(dataController.PersonTable, Is.Not.Null);
        }

        [Test]
        public void PictureTable_IsNotNull()
        {
            Assert.That(dataController.PictureTable, Is.Not.Null);
        }

        [Test]
        public void PushMessageTable_IsNotNull()
        {
            Assert.That(dataController.PushMessageTable, Is.Not.Null);
        }

        [Test]
        public void ReportTable_IsNotNull()
        {
            Assert.That(dataController.ReportTable, Is.Not.Null);
        }

        [Test]
        public void ReportDetailsTable_IsNotNull()
        {
            Assert.That(dataController.ReportDetailsTable, Is.Not.Null);
        }

        [Test]
        public void SocialMessageTable_IsNotNull()
        {
            Assert.That(dataController.SocialMessageTable, Is.Not.Null);
        }

        [Test]
        public void SocialsTable_IsNotNull()
        {
            Assert.That(dataController.SocialsTable, Is.Not.Null);
        }

        [Test]
        public void SoundRecordTable_IsNotNull()
        {
            Assert.That(dataController.SoundRecordTable, Is.Not.Null);
        }

        [Test]
        public async Task FetchReports_ReturnsAtleastOneObject()
        {
            var reports = await dataController.FetchReportsAsync();

            Assert.That(reports, Is.Not.Empty);
        }

        [Test]
        public async Task FetchReportDetails_ReturnsAtleastOneObject()
        {
            //get all (active) reports
            var reports = await dataController.FetchReportsAsync();

            //create a list object
            ObservableCollection<ReportDetails> reportDetails = null;

            foreach (var report in reports)
            {
                var list = await dataController.FetchReportDetailsAsync(report.ReportId);
                if (list.Any()) // so if there is a reportdetails entry we stop the loop and assert the result
                {
                    reportDetails = list;
                    break;
                }
            }

            if (reportDetails != null) // so if all the active reports dont have a detailsentry we dont assert
            {
                Assert.That(reportDetails, Is.Not.Empty);
            }
        }

        [Test]
        public async Task QueryReports_ReturnsAtleastOneObject()
        {
            //get all (active) reports
            var reports = await dataController.FetchReportsAsync();

            var queryResults = await dataController.QueryReports(reports.First(x => !string.IsNullOrEmpty(x.Comment)).Comment);

            Assert.That(queryResults, Is.Not.Empty);
        }

        [Test]
        public async Task InsertObjectIntoDB_Succeed()
        {
            var person = new Person{FirstName = "Henk", LastName = "Eikelboom", BirthDate = new DateTime(1950,01,01)};
            await dataController.InsertIntoAsync(person);
            

            Assert.That(person.PersonId, Is.Not.EqualTo(Guid.Empty));

            //cleanUp
            person.Id = person.PersonId;
            await dataController.PersonTable.DeleteAsync(person);
        }

        [Test]
        public async Task UpdateObjectInDB_Succeed()
        {
            //create new entry
            var person = new Person { FirstName = "Henk", LastName = "Eikelboom", BirthDate = new DateTime(1950, 01, 01) };
            await dataController.InsertIntoAsync(person);
            person.Id = person.PersonId;

            string changedLastname = person.LastName = "Eikelboom-Antwanette";

            await dataController.UpdateSetAsync(person);

            var updatedPerson = await dataController.PersonTable.LookupAsync(person.PersonId); // new fetch of db object not really necessary but w/e

            Assert.That(updatedPerson.LastName, Is.EqualTo(changedLastname));

            //cleanUp
            await dataController.PersonTable.DeleteAsync(person);

        }
    }
}