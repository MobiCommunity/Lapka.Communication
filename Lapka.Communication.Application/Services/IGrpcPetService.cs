using System;
using System.Threading.Tasks;

namespace Lapka.Communication.Application.Services
{
    public interface IGrpcPetService
    {
        Task<Guid> GetShelterIdAsync(Guid petId);
    }
}