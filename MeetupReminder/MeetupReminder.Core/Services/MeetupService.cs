using CSharp.Meetup.Connect;
using Spring.Social.OAuth1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupReminder;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using MeetupReminder.Core.Domain;
using System.Collections.ObjectModel;

namespace MeetupReminder.Core.Services
{
    public class MeetupService
    {
        private const string MeetupApiKey = "bkgqr5siado16t6i1ed5cgrq3u";
        private const string MeetupSecretKey = "nc9pcqo0i0ibcpvv1ojlhmd14c";
        

        public static async Task<OAuthToken> authenticate()
        {
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);

            var oauthToken = meetupServiceProvider.OAuthOperations.FetchRequestTokenAsync("oob", null).Result;
            var authenticateUrl = meetupServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
            Process.Start(authenticateUrl);
            return oauthToken;
        }

        public static async Task<OAuthToken> authenticate2(OAuthToken tok, string pin)
        {
            var requestToken = new AuthorizedRequestToken(tok, pin);
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);
            var oathAccessToken = meetupServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            return oathAccessToken;
        }

        public static async Task<ObservableCollection<MeetupEvent>> GetMeetupsFor(string uservar, string toggle, OAuthToken token)
        {
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);
            var meetup = meetupServiceProvider.GetApi(token.Value, token.Secret);
            string json;
            if (toggle == "Meetup Group")
            {
                json = await meetup.RestOperations.GetForObjectAsync<string>($"https://api.meetup.com/2/events?offset=0&format=json&limited_events=False&group_urlname={uservar}&photo-host=public&page=20&fields=&order=time&desc=false&status=upcoming&sig_id=182757480&sig=52d73860784c45691862aa492856c26332cd67af");
            }
            else
            {
                json = await meetup.RestOperations.GetForObjectAsync<string>($"https://api.meetup.com/2/concierge?zip={uservar}&offset=0&format=json&photo-host=public&page=500&sig_id=182757480&sig=199b985b4335074f44fbe4697f34f1ff7026bf50");
            }
            EventReturn j = JsonConvert.DeserializeObject<EventReturn>(json);
            /*   ObservableCollection<string> MEvents = new ObservableCollection<string>();
               foreach (var i in j.results)
               {
                   MEvents.Add($"\nName: {i.name}\nStatus: {i.status}\nTime: {i.time}");
               }*/
            ObservableCollection<MeetupEvent> MEvents = new ObservableCollection<MeetupEvent>();
            foreach (var i in j.results)
            {
                MEvents.Add(i);
            }

            return MEvents;
        }

    }
}
