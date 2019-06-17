using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeIoTHub.Server.Services
{
    public class FcmService : IFcmService
    {
        public async Task SendToAll(string title, string message)
        {
            var fcmKey = "AAAARPAmr2k:APA91bHeZYSnPBQgjKgj4mWbSMpOZbvA67VCxUTOLJpVRLCLFThTmV4UuVu8TGP7zrCxRnPoyAPDNvWL1FImztf5vty8Z4hIRZ73UiY8AcwyovUm8uLPVEtfmsA0-ykbPd1CS0bLoMZa";
            var client = new HttpClient(new LoggingHandler(new HttpClientHandler()));


            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={fcmKey}");

            var content = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    to = "/topics/dogwater",
                    //to = "/dogwater",
                    //to = "ew76ubw1Wzk:APA91bGZJPhjJFV17GOr7n9HqVte5K23aO3InMn9714_sqeyBr1po--_3d6gHxNW45fZTlY8nn7-Yrftt5BJD1oarExeClmYbNfKWgETt5HVoF2m67YtBnzA-YOtihAvaaGfxlWpEE9B",
                    notification = new { body = message, title },
                    data = new { body = message, title }
                }), Encoding.UTF8, "application/json");


            var response = await client.PostAsync("https://fcm.googleapis.com/fcm/send", content);
            Debug.WriteLine(response.StatusCode + " " + await response.Content.ReadAsStringAsync());
        } 
    }

    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());
            if (response.Content != null)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            Console.WriteLine();

            return response;
        }
    }
}
