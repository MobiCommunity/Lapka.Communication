using System;
using System.Threading.Tasks;

namespace Lapka.Communication.Application.Services
{
    public interface IGrpcIdentityService
    {
        Task<bool> IsUserOwnerOfShelterAsync(Guid shelterId, Guid userId);
        Task<Guid> ClosestShelter(string longitude, string latitude);
    }
}