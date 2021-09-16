using System;
using System.Threading.Tasks;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcIdentityService
    {
        Task<bool> IsUserOwnerOfShelterAsync(Guid shelterId, Guid userId);
        Task<Guid> ClosestShelterAsync(string longitude, string latitude);
        Task<bool> DoesShelterExists(Guid shelterId);
    }
}