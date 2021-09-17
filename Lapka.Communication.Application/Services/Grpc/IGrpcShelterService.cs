using System;
using System.Threading.Tasks;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcShelterService
    {
        Task<Location> GetShelterLocationAsync(Guid shelterId);
    }
}