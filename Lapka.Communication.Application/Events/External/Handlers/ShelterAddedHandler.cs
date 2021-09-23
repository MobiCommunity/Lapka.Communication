using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Events.External.Handlers
{
    public class ShelterAddedHandler : IEventHandler<ShelterAdded>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcShelterService _grpcShelterService;

        public ShelterAddedHandler(IShelterRepository shelterRepository, IGrpcShelterService grpcShelterService)
        {
            _shelterRepository = shelterRepository;
            _grpcShelterService = grpcShelterService;
        }

        public async Task HandleAsync(ShelterAdded @event)
        {
            Location location;
            try
            {
                location = await _grpcShelterService.GetShelterLocationAsync(@event.ShelterId);
            }
            catch (Exception ex)
            {
                throw new CannotRequestIdentityMicroserviceException(ex);
            }

            Shelter shelter = Shelter.Create(@event.ShelterId, location);

            await _shelterRepository.CreateAsync(shelter);
        }
    }
}