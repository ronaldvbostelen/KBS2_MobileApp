using System.Threading;
using System.Threading.Tasks;
using KBS2.WijkagentApp.Models.DataControllers;
using NUnit.Framework;

namespace KBS2.WijkagentApp.UnitTests
{
    [TestFixture]
    public class TwitterDataControllerTests
    {
        private TwitterDataController twitterDataController;
        
        public TwitterDataControllerTests()
        {
            twitterDataController = new TwitterDataController();
        }

        [Test]
        public async Task TweetQuery_WillYieldSomeResults()
        {
            // yeeeeeeeeeah, So the twittercontroller initialise async. so this methode will be put on the stack before
            // the twittercontroller is fully initialized. we cant check whether the inittask has completed (cause of private)
            // so we delay this method. in 2.5 sec the twittercontroller runs and we can use its methods. (parallel programming ftw)
            Thread.Sleep(2500); 

            var tweets = await twitterDataController.GetTweetsInRadius(52.5168, 6.0830, 25, "Zwolle","Peperbus");
            
            Assert.That(tweets, Is.Not.Empty);
        }
    }
}