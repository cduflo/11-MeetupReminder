using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupReminderConsole.Core.Services
{
    public class SmsService
    {
        private const string TwilioKey = "AC19d0a1b95d41aa34f8d02e038e77cc59";
        private const string TwilSecret = "cf7e706af8127b6c7cf99d33b55280f7";
        private const string FromNumber = "+18606501869";

        public static void SendSms(string to, string message)
        {
            var twilio = new Twilio.TwilioRestClient(TwilioKey, TwilSecret);
            twilio.SendMessage(FromNumber, to, message);
        }

        public static void Call(string to, string message)
        {
            var twilio = new Twilio.TwilioRestClient(TwilioKey, TwilSecret);
            var call = twilio.InitiateOutboundCall(FromNumber, to, message);
        }
    }
}
