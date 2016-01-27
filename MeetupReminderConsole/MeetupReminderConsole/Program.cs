using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupReminderConsole.Core.Services;


namespace MeetupReminderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string group = "";
            string toggle = "";

            while (true)
            {
                Console.WriteLine("Do you want to search by Meetup or by Zip Code? Enter 'meetup' or 'zip'.");
                toggle = Console.ReadLine().ToLower();

                if (toggle == "meetup")
                {
                    Console.WriteLine("What meetup do you want to know about?");
                    group = Console.ReadLine();
                    break;
                }
                else if (toggle == "zip")
                {
                    Console.WriteLine("What Zip Code do you want to know about?");
                    group = Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Please make your seleciton by typing 'meetup' or 'zip'");
                }

            }

            Console.WriteLine();
            Console.WriteLine("If you would like the results sent to your phone? Please enter your number.");
            string toNum = Console.ReadLine();

            var meetups = MeetupService.GetMeetupsFor(group, toggle).Result;
            var message = "Here are your requested meetups:";

            foreach (var oEvent in meetups)
            {
                message += "\nMeetup for you - " + oEvent;
            }

            Console.WriteLine();
            Console.WriteLine(message);
            SmsService.SendSms(toNum, message);

            Console.ReadLine();
        }
    }
}
