using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Core.ValueObjects;

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

        public async Task<ShelterPet> GetPetBasicInfoAsync(Guid petId)
        {
            GetShelterPetBasicInfoReply response = await _client.GetShelterPetBasicInfoAsync(
                new GetShelterPetBasicInfoRequest
                {
                    PetId = petId.ToString()
                });

            return new ShelterPet(petId, response.PetName, response.Race, DateTime.Parse(response.BirthDate),
                response.PhotoPaths);
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