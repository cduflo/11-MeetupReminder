using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupReminder.Core.Domain
{
    public class Reference
    {
        public static string[] queryType = new string[] { "Zip Code", "Meetup Group" };

    }

    public class EventReturn
    {
        public List<MeetupEvent> results { get; set; }
    }

    public class MeetupEvent
    {
        public string name { get; set; }
        public string status { get; set; }
        public string time { get; set; }
    }
}
