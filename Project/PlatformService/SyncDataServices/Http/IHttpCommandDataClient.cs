using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface IHttpCommandDataClient
    {
        Task SendRequestToCommandsService(PlatformReadDto Platform);
    }
}