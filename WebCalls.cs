using System;
using System.Net.Http;
using System.Globalization;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

namespace Dullahan
{

    public class WebCalls
    {
        public const string DEVURL = "http://localhost:5000";

        public const string LOGSCAN = "/api/inventory/scan";

        public const string FK_LOCATION_ROOM_READER_ID = "0001";

        private int LOGSCAN_RATE = 2500;

        // Time of last call trackers
        // private (int, DateTime) lastScan;
        // Dictionary of scan values and last time it was sent as a request
        // private Dictionary<string, DateTime> lastScan = new Dictionary<string, DateTime>();

        public WebCalls()
        {

        }

        public void SetRate(int newRate) 
        {
            LOGSCAN_RATE = newRate;
        }
        public async Task<String> ScanCall(String[] scanPackage, HttpClient client, String URL)
        {
            // The scan call will only need the scan data and will assign a timestamp on the serverside
            // Previous problems:
            // content was empty with FormURL encoding, now uses JsonSerializer
            Console.WriteLine("Generating payload \n");
            Console.WriteLine("scanPackage: " + scanPackage);
            var payloadJSON = new PostData
            {
                SCANS = scanPackage,
                FK_LOCATION_ROOM_READER_ID = FK_LOCATION_ROOM_READER_ID
            };

            // 4.5.2 missing namespace json
            var json = JsonSerializer.Serialize(payloadJSON);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(URL + LOGSCAN, payload);
                var responseString = await response.Content.ReadAsStringAsync();
                return "SUCCESS";
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HTTP REQUEST ERROR: " + e);
                return "HTTP REQUEST ERROR";
            }

        }
        public bool Debouncer(string ENDPOINT, String[] data)
        {
            // Get time passed between last call and current call
            DateTime currentTime = DateTime.Now;
            DateTime firstTime = DateTime.Parse(data[1]);

            TimeSpan timeDiff = currentTime.Subtract(firstTime);
            int timeDiffMS = (int)timeDiff.TotalMilliseconds;
            Console.WriteLine("TIMEDIFFMS: " + timeDiffMS);

            // Continue if enough time has passed
            if (timeDiffMS > RateLimiter(ENDPOINT))
            {
                Console.WriteLine("Rate buffer reached \n");
                return true;
            }

            // Not enough time has passed
            else
            {
                Console.WriteLine(timeDiffMS);

                return false;
            }
        }

        private int RateLimiter(string ENDPOINT)
        {
            // Return ints are values in milliseconds

            switch (ENDPOINT)
            {
                case LOGSCAN:
                    return LOGSCAN_RATE;
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}

