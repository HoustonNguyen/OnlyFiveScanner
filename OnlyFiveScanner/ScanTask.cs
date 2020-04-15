using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyFiveScanner
{
    class ScanTask
    {
        private int StartingNumber, RangeToCheck;
        private string BASEURL;
        private List<string> CREATOR_NAMES;

        public ScanTask(int startNumber, int RangeToCheck, string BASEURL, List<string> creatorNamesToFilterFor)
        {
            this.StartingNumber = startNumber;
            this.RangeToCheck = RangeToCheck;
            this.BASEURL = BASEURL;
            this.CREATOR_NAMES = creatorNamesToFilterFor;
        }

        public int Run()
        {
            int NumberOfTimesFailed = 0;
            int  lastSuccessfulCounterNumber = StartingNumber;
            string lastSuccessfulURL = $"{BASEURL}{lastSuccessfulCounterNumber.ToString()}";

            int counter = StartingNumber;
            while (counter <= StartingNumber + RangeToCheck)
            {
                try
                {
                    string activetestURL = $"{BASEURL}{counter.ToString()}";
                    //Log("Testing URL " + activetestURL);

                    var webClient = new HtmlWeb();
                    //string downloadedHTMLString = webClient.DownloadString();

                    var htmlDoc = webClient.Load(activetestURL);

                    var creatorNameH2Container = htmlDoc.DocumentNode
                        .SelectSingleNode("//h2[contains(@class, 'creator-name')]");

                    var container404 = htmlDoc.DocumentNode
                        .SelectSingleNode("//div[contains(@class, 'notfound')]");

                    if (container404 == null && creatorNameH2Container != null && creatorNameH2Container.InnerHtml != null && string.IsNullOrWhiteSpace(creatorNameH2Container.InnerHtml) == false)
                    {
                        lastSuccessfulURL = activetestURL;
                        lastSuccessfulCounterNumber = counter;
                        if (CREATOR_NAMES.Any(creatorName => creatorNameH2Container.InnerHtml.Trim().Replace(" ", string.Empty).Contains(creatorName)))
                        {
                            //Log($"Found {creatorNameH2Container.InnerHtml} at {activetestURL}!");
                            var publishDateDIVContainer = htmlDoc.DocumentNode
                                .SelectSingleNode("//*[contains(@class, 'published-date')]");

                            var sellStatus = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'post-container')]/div[contains(@class, 'image-wrapper')]/div[contains(@class, 'sold-out')]");
                            if (sellStatus != null)
                            {
                                Log($"(SOLD OUT) {creatorNameH2Container.InnerHtml} {activetestURL}");
                                counter++;
                                continue;
                            }

                            if (publishDateDIVContainer != null && publishDateDIVContainer.InnerHtml != null && string.IsNullOrWhiteSpace(publishDateDIVContainer.InnerHtml) == false)
                            {
                                string dateTimeString = publishDateDIVContainer.InnerHtml.Replace("に公開", string.Empty);
                                DateTime? publishedDateTime = null;
                                try
                                {
                                    publishedDateTime = DateTime.Parse(dateTimeString).AddHours(-9);
                                }
                                catch (Exception)
                                {
                                    counter++;
                                    continue;
                                }
                                if (publishedDateTime.HasValue == true)
                                {
                                    //Convert to eastern time
                                    var localMachineTimeZone = TimeZoneInfo.Local;
                                    var publishedDateTimeInEST = TimeZoneInfo.ConvertTimeFromUtc(publishedDateTime.Value, localMachineTimeZone);
                                    string isSoldOutString = (sellStatus == null ? string.Empty : "(SOLD OUT)");
                                    string isPastOrFutureString = ((publishedDateTimeInEST < DateTime.Now) ? "(Past)" : "(Future)");

                                    Log($"{isSoldOutString}ONLYFIVE - {creatorNameH2Container.InnerHtml} @ {publishedDateTimeInEST.ToString()}{isPastOrFutureString} {activetestURL}");
                                    NumberOfTimesFailed = 0;
                                }
                            }
                        }
                    }
                    else if (container404 != null)
                    {
                        //Log($"{activetestURL} failed");
                        NumberOfTimesFailed++;
                        if (NumberOfTimesFailed >= 25)
                        {
                            Log($"Failed too many times in a row. Last successful URL was {lastSuccessfulURL}. I don't think there are any posts after that one, yet.");
                            break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    NumberOfTimesFailed++;
                    Log(ex.Message.Substring(0, Math.Min(ex.Message.Length, 100)));
                }

                counter++;
            }
            Log($"Searching stopped - Was going to stop at posts/{StartingNumber + RangeToCheck} - Looked up to posts/{counter} - Made {counter - StartingNumber} requests");
            return lastSuccessfulCounterNumber;
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} - {message}");
        }
    }
}
