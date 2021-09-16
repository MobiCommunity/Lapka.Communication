using System;
using System.Threading.Tasks;

namespace Lapka.Communication.Application.Services.Grpc
{
    public interface IGrpcPetService
    {
        Task<Guid> GetShelterIdAsync(Guid petId);
    }
}