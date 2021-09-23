using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services.Grpc;

namespace Lapka.Communication.Infrastructure.Grpc.Services
{
    public class GrpcPetService : IGrpcPetService
    {
        private readonly PetProto.PetProtoClient _client;

        public GrpcPetService(PetProto.PetProtoClient client)
        {
            _client = client;
        }
        
        public async Task<Guid> GetShelterIdAsync(Guid petId)
        {
            GetPetsShelterReply response = await _client.GetPetsShelterAsync(new GetPetsShelterRequest
            {
                PetId = petId.ToString()
            });

            Guid shelterId = GetShelterIdAsGuid(petId, response.ShelterId);

            return shelterId;
        }

        private static Guid GetShelterIdAsGuid(Guid petId, string unconvertedShelterId)
        {
            if (!Guid.TryParse(unconvertedShelterId, out Guid shelterId))
            {
                throw new PetDoesNotExistsException(petId);
            }

            return shelterId;
        }
    }
}