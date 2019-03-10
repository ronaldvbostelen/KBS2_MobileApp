using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToTwitter;

namespace KBS2.WijkagentApp.Models.DataControllers
{
    class TwitterDataController
    {
        private TwitterContext TwitterContext { get; set; }

        public TwitterDataController()
        {
            var init = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = "yFdnfYXOZmH5Z15qYk6srt2E6",
                    ConsumerSecret = "6wOk2naxqyRd4xwhJlkfmagYuNxvbLZjUhILyOWxmgBAU1byf4"
                }
            };
            
            await auth.AuthorizeAsync();

            TwitterContext = new TwitterContext(auth);
        }

        // with this method we fetch tweets from the twitter api in a certain long-lat in a radius with matching words, words are seperated with OR
        public async Task<List<Status>> GetTweetsInRadius(double latitude, double longitude, double radius, params string[] parms)
        {
            // create a new string with OR operander
            var queryParms = new StringBuilder();

            /*
             * loop through parms, if it got value we remove the non alphabetic characters and add them to the search string with a OR operander. 
             * last word will also contain a OR but twitter api ignores it, so we dont bother
             */
            foreach (var queryParameter in parms)
            {
                if (!string.IsNullOrWhiteSpace(queryParameter))
                {
                    var parmsArray = queryParameter.Split(' ');
                    foreach (var parm in parmsArray)
                    {
                        var alphabeticOnly = Regex.Replace(parm, @"[^a-zA-Z]", "");
                        queryParms.Append(alphabeticOnly);
                        queryParms.Append(" OR ");
                    }
                }
            }
            
            /*
             * query is from linqtotwitter framework. we search the extended tweets (not turnacted), Recent. well twitter api goes back 7 days.
             * the searchstring is created above and we add the geocode based of the provided arguments
             */
            var query = await (
                from   search in TwitterContext.Search
                where  search.Type       == SearchType.Search     &&
                       search.TweetMode  == TweetMode.Extended    &&
                       search.ResultType == ResultType.Recent     &&
                       search.Query      == queryParms.ToString() &&
                       search.GeoCode    == $"{latitude},{longitude},{radius}"
                select search).SingleOrDefaultAsync();

            return query.Statuses;
        }
    }
}
