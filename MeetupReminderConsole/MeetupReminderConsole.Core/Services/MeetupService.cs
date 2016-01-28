using CSharp.Meetup.Connect;
using Newtonsoft.Json.Linq;
using Spring.Social.OAuth1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupReminderConsole.Core.Services
{
    public class MeetupService
    {
        private const string MeetupApiKey = "bkgqr5siado16t6i1ed5cgrq3u";
        private const string MeetupSecretKey = "nc9pcqo0i0ibcpvv1ojlhmd14c";

        private static async Task<OAuthToken> authenticate()
        {
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);
            //OAuth Dance//
            var oauthToken = meetupServiceProvider.OAuthOperations.FetchRequestTokenAsync("oob", null).Result;
            Console.WriteLine(oauthToken.GetType().ToString());
            var authenticateUrl = meetupServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
            Process.Start(authenticateUrl);

            Console.WriteLine("Enter the pin from meetup.com");
            string pin = Console.ReadLine();

            var requestToken = new AuthorizedRequestToken(oauthToken, pin);
            var oathAccessToken = meetupServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            return oathAccessToken;

        }

        public static async Task<List<string>> GetMeetupsFor(string uservar, string toggle)
        {
            var token = await authenticate();
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);
            var meetup = meetupServiceProvider.GetApi(token.Value, token.Secret);
            string json;
            if (toggle == "meetup")
            {
                json = await meetup.RestOperations.GetForObjectAsync<string>($"https://api.meetup.com/2/events?offset=0&format=json&limited_events=False&group_urlname={uservar}&photo-host=public&page=20&fields=&order=time&desc=false&status=upcoming&sig_id=182757480&sig=52d73860784c45691862aa492856c26332cd67af");
            }
            else
            {
                json = await meetup.RestOperations.GetForObjectAsync<string>($"https://api.meetup.com/2/concierge?zip={uservar}&offset=0&format=json&photo-host=public&page=500&sig_id=182757480&sig=199b985b4335074f44fbe4697f34f1ff7026bf50");
            }
            var oEvents = JObject.Parse(json)["results"];
            List<string> MEvents = new List<string>();
            foreach (var oEvent in oEvents)
            {
                MEvents.Add(oEvent["name"].ToString());
            }

            return MEvents;
        }
    }
}
