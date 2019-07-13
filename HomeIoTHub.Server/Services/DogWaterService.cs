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
        int lastLevel = 0;

        public DogWaterService(IFcmService fcmService)
        {
            this.fcmService = fcmService;
        }

        public async Task HandleDogWaterEvent(MqttApplicationMessage message)
        {
            var messageText = Encoding.UTF8.GetString(message.Payload);

            if (int.TryParse(messageText, out int level))
            {
                if (level == 0 && lastLevel == 1)
                {
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + "\tThe dog needs water!");
                    
                    // Send a push notification.
                    await fcmService.SendToAll("The Dog Needs Water!", "The office water dish is low.");
                }

                lastLevel = level;
            }

        }
    }
}
