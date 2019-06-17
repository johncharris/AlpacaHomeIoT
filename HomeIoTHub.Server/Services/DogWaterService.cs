using HomeIoTHub.Server.Services;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeIoTHub.Server
{
    public class DogWaterService : IDogWaterService
    {
        readonly IFcmService fcmService;

        public DogWaterService(IFcmService fcmService)
        {
            this.fcmService = fcmService;
        }

        public async Task HandleDogWaterEvent(MqttApplicationMessage message)
        {
            var messageText = Encoding.UTF8.GetString(message.Payload);

            int level = 0;
            if (int.TryParse(messageText, out level))
            {
                var percent = ((level * 100) / 270);
                if (percent < 25)
                {
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + "\tThe dog needs water!");
                    
                    // Send a push notification.
                    await fcmService.SendToAll("The Dog Needs Water!", "The office water dish is low.");
                }
            }

        }
    }
}
