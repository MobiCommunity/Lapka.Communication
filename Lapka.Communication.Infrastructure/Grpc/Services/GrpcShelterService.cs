using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Infrastructure.Grpc.Services
{
    public class GrpcShelterService : IGrpcShelterService
    {
        private readonly ShelterProto.ShelterProtoClient _client;

        public GrpcShelterService(ShelterProto.ShelterProtoClient client)
        {
            _client = client;
        }
        public async Task<Location> GetShelterLocationAsync(Guid shelterId)
        {
            GetShelterLocationReply response = await _client.GetShelterLocationAsync(new GetShelterLocationRequest
            {
                ShelterId = shelterId.ToString(),
            });

            Location location = new Location(response.Latitude, response.Longitude);

            return location;
        }
    }
}