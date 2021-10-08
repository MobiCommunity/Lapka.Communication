using System;
using System.Threading.Tasks;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcPetService
    {
        Task<Guid> GetShelterIdAsync(Guid petId);
        Task<ShelterPet> GetPetBasicInfoAsync(Guid petId);
    }
}