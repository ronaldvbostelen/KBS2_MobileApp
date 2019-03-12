using System.Net;
using System.Threading.Tasks;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models.DataControllers;
using NUnit.Framework;

namespace KBS2.WijkagentApp.UnitTests
{
    [TestFixture]
    public class LoginValidationTests
    {
        private DataController dataController;
        
        public LoginValidationTests()
        {
            dataController = new DataController();
        }

        [TestCase("Agent101", "1234", HttpStatusCode.OK, Description = "right credentials")]
        [TestCase("Agent102", "1234", HttpStatusCode.OK, Description = "right credentials")]
        [TestCase("Agent104", "Wachtwoord", HttpStatusCode.OK, Description = "right credentials")]
        [TestCase("Agent104", "wachtwoord", HttpStatusCode.NotFound, Description = "wrong credentials")]
        [TestCase("Agent1O2", "1234", HttpStatusCode.NotFound, Description = "wrong credentials")]
        [TestCase("11111111111111", "1234", HttpStatusCode.NotFound, Description = "wrong credentials")]
        [TestCase("Agent", "3333", HttpStatusCode.NotFound, Description = "wrong credentials")]
        [TestCase("", "", HttpStatusCode.BadRequest, Description = "empty credentials")]
        [TestCase(null, null, HttpStatusCode.BadRequest, Description = "null credentials")]
        [TestCase("     ", "     ", HttpStatusCode.BadRequest, Description = "only space credentials")]
        public async Task Login_Validation(string loginName, string password, HttpStatusCode statusCode)
        {
            var checkLoginCredentialsTask = dataController.CheckOfficerCredentialsAsync(new Officer {UserName = loginName, Password = password});
            var loginCheck = await checkLoginCredentialsTask;

            Assert.That(loginCheck.StatusCode, Is.EqualTo(statusCode));
        }
    }
}
