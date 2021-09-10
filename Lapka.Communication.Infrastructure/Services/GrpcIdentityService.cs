﻿using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Services;

namespace Lapka.Communication.Infrastructure.Services
{
    public class GrpcIdentityService : IGrpcIdentityService
    {
        private readonly IdentityProto.IdentityProtoClient _client;

        public GrpcIdentityService(IdentityProto.IdentityProtoClient client)
        {
            _client = client;
        }
        public async Task<bool> IsUserOwnerOfShelterAsync(Guid shelterId, Guid userId)
        {
            IsUserOwnerOfShelterReply response = await _client.IsUserOwnerOfShelterAsync(new IsUserOwnerOfShelterRequest
            {
                ShelterId = shelterId.ToString(),
                UserId = userId.ToString()
            });

            return response.IsOwner;
        }

        public async Task<Guid> ClosestShelterAsync(string longitude, string latitude)
        {
            GetClosestShelterReply response = await _client.GetClosestShelterAsync(new GetClosestShelterRequest
            {
                Longitude = longitude,
                Latitude = latitude
            });
            
            if(!Guid.TryParse(response.ShelterId, out Guid shelterId))
            {
                return Guid.Empty;
            }

            return shelterId;
        }

        public async Task<bool> DoesShelterExists(Guid shelterId)
        {
            
            DoesShelterExistsReply response = await _client.DoesShelterExistsAsync(new DoesShelterExistsRequest
            {
                ShelterId = shelterId.ToString()
            });

            return response.DoesExists;
        }
    }
}