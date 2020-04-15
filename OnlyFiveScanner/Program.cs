using System;
using System.Collections.Generic;

namespace OnlyFiveScanner
{
    class Program
    {
        static int StartingNumber = 2723;//2256;
        static int RangeToCheck = 500;
        static readonly string BASEURL = "https://only-five.jp/posts/";
        static readonly List<string> CREATOR_NAMES = new List<string>()
        {
            "伊崎るな",
            "瀬戸そらら",
            "姫乃ななみ",
            "沙倉あやの",
            "八神なゆた"
        };

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string entry = "CONTINUE";

            Console.Write("Enter a starting number: posts/");
            string numberString = Console.ReadLine();
            int startNumber;

            while (int.TryParse(numberString, out startNumber) == false)
            {
                Console.Write("Enter a starting number: posts/");
                numberString = Console.ReadLine();
            }


            StartingNumber = startNumber;

            while (entry.Equals("STOP") == false)
            {
                Log($"Searching from posts/{StartingNumber}");
                ScanTask st = new ScanTask(StartingNumber, RangeToCheck, BASEURL, CREATOR_NAMES);
                StartingNumber = st.Run();

                Console.WriteLine($"Press any key to run again or enter STOP to stop and terminate. Running again will start at where we left off: {StartingNumber.ToString()}");
                entry = Console.ReadLine();
            }

            entry = entry.ToUpper().Trim();

            //Log($"Program reached end. Press any key to terminate.");
        }

        static void Log(string message) 
        { 
            Console.WriteLine($"{DateTime.Now.ToString()} - {message}");
        }

        static void InitializeSMTPClient()
        {
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send email on the client's behalf.
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("example@gmail.com", "<example-password>");
        }

        static void BuildOutInviteFile()
        {
            //StringBuilder sb = new StringBuilder();
            //string DateFormat = "yyyyMMddTHHmmssZ";
            //string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);

            //sb.AppendLine("BEGIN:VCALENDAR");
            //sb.AppendLine("PRODID:-//Compnay Inc//Product Application//EN");
            //sb.AppendLine("VERSION:2.0");
            //sb.AppendLine("METHOD:PUBLISH");

            //sb.AppendLine("BEGIN:VEVENT");
            //sb.AppendLine("DTSTART:" + publishedDateTimeInEST.ToUniversalTime().ToString(DateFormat));
            //sb.AppendLine("DTEND:" + publishedDateTimeInEST.ToUniversalTime().ToString(DateFormat));
            //sb.AppendLine("DTSTAMP:" + now);
            //sb.AppendLine("UID:" + Guid.NewGuid());
            //sb.AppendLine("CREATED:" + now);
            ////sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + res.DetailsHTML);
            //sb.AppendLine("DESCRIPTION:" + $"ONLYFIVE - {creatorNameH2Container.InnerHtml} @ {publishedDateTimeInEST.ToString()}{((publishedDateTimeInEST < DateTime.Now) ? "(Past)" : "(Future)")} {activetestURL}");
            //sb.AppendLine("LAST-MODIFIED:" + now);
            //sb.AppendLine("LOCATION:" + activetestURL);
            //sb.AppendLine("SEQUENCE:0");
            //sb.AppendLine("STATUS:CONFIRMED");
            ////sb.AppendLine("SUMMARY:" + res.Summary);
            //sb.AppendLine("TRANSP:OPAQUE");
            //sb.AppendLine("END:VEVENT");
            //sb.AppendLine("END:VCALENDAR");

            //var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());
            //using (MemoryStream ms = new MemoryStream(calendarBytes))
            //{
            //    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");
            //    message.Attachments.Add(attachment);

            //    System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            //    contype.Parameters.Add("method", "REQUEST");
            //    //  contype.Parameters.Add("name", "Meeting.ics");
            //    AlternateView avCal = AlternateView.CreateAlternateViewFromString(sb.ToString(), contype);
            //    message.AlternateViews.Add(avCal);

            //    client.Send(message);

            //}
        }
    }
}
