using System;
using System.Threading.Tasks;

namespace Lapka.Communication.Application.Services
{
    public interface IGrpcPetService
    {
        Task<bool> DoesPetExists(Guid petId);
    }
}