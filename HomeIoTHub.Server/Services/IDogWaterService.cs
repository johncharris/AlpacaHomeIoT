using System.Threading.Tasks;
using MQTTnet;

namespace HomeIoTHub.Server
{
    public interface IDogWaterService
    {
        Task HandleDogWaterEvent(MqttApplicationMessage message);
    }
}