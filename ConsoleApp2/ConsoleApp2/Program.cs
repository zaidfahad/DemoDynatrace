﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        // An example JSON object, with key/value pairs
        static readonly string Json = @"[{""Name"":""Zaid"",""DemoField2"":""Khan""},{""DemoField3"":""Cloud Expert"",""DemoField4"":""Indian""}]";

        // Update customerId to your Log Analytics workspace ID
        static readonly string CustomerId = "f68788d2-0dc0-430d-b9d2-78fab0c70399";

        // For sharedKey, use either the primary or the secondary Connected Sources client authentication key   
        static readonly string SharedKey = "2Cv8Yj8PXkii9/oWUGfrZhyPu2+7BhCx/oQsnZhORCbqwgSYtMAPIJqiNVu115a/Xw+Nuw+dgqPIgquV5GfcYw==";

        // LogName is name of the event type that is being submitted to Azure Monitor
        static readonly string LogName = "DyatraceLog";

        // You can use an optional field to specify the timestamp from the data. If the time field is not specified, Azure Monitor assumes the time is the message ingestion time
        static readonly string TimeStampField = DateTime.UtcNow.ToString("r");
        // DateTime dt = new DateTime();
        static void Main()
        {
            bool test = true;
            // Create a hash for the API signature
            string datestring  = DateTime.UtcNow.ToString("r");
            var jsonBytes = Encoding.UTF8.GetBytes(Json);
            string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            string hashedString = BuildSignature(stringToHash, SharedKey);
            string signature = "SharedKey " + CustomerId + ":" + hashedString;
            //while (test)
            //{
            PostData(signature, datestring, Json);
            //}
        }
        private static void TimerCallback(Object o)
        {
            Console.WriteLine("In TimerCallback: " + DateTime.Now);
        }
        // Build the API signature
        public static string BuildSignature(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Send a request to the POST API endpoint
        public static void PostData(string signature, string date, string json)
        {
            try
            {
                string url = "https://" + CustomerId + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Log-Type", LogName);
                client.DefaultRequestHeaders.Add("Authorization", signature);
                client.DefaultRequestHeaders.Add("x-ms-date", date);
                client.DefaultRequestHeaders.Add("time-generated-field", TimeStampField);

                // If charset=utf-8 is part of the content-type header, the API call may return forbidden.
                System.Net.Http.HttpContent httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Task<System.Net.Http.HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

                System.Net.Http.HttpContent responseContent = response.Result.Content;
                string result = responseContent.ReadAsStringAsync().Result;
                Console.WriteLine("Return Result: " + result);
            }
            catch (Exception excep)
            {
                Console.WriteLine("API Post Exception: " + excep.Message);
            }
        }
    }
}
