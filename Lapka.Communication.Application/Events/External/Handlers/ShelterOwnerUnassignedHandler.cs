using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Events.External.Handlers
{
    public class ShelterOwnerUnassignedHandler : IEventHandler<ShelterOwnerUnassigned>
    {
        private readonly IShelterRepository _shelterRepository;

        public ShelterOwnerUnassignedHandler(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(ShelterOwnerUnassigned @event)
        {
            Shelter shelter = await _shelterRepository.GetAsync(@event.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(@event.ShelterId);
            }
            
            shelter.RemoveOwner(@event.UserId);
            await _shelterRepository.UpdateAsync(shelter);
        }
    }
}