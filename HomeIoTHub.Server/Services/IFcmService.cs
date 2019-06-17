using System.Threading.Tasks;

namespace HomeIoTHub.Server.Services
{
    public interface IFcmService
    {
        Task SendToAll(string title, string message);
    }
}