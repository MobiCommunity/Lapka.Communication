using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Services;

namespace Lapka.Communication.Infrastructure.Services
{
    public class GrpcPetService : IGrpcPetService
    {
        private readonly PetProto.PetProtoClient _client;

        public GrpcPetService(PetProto.PetProtoClient client)
        {
            _client = client;
        }
        
        public async Task<bool> DoesPetExists(Guid petId)
        {
            DoesPetExistsReply response = await _client.DoesPetExistsAsync(new DoesPetExistsRequest
            {
                PetId = petId.ToString()
            });

            return response.DoesExists;
        }
    }
}