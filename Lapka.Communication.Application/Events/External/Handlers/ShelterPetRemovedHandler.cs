using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Events.External.Handlers
{
    public class ShelterPetRemovedHandler : IEventHandler<ShelterPetRemoved>
    {
        private readonly IShelterPetRepository _shelterPetRepository;

        public ShelterPetRemovedHandler(IShelterPetRepository shelterPetRepository)
        {
            _shelterPetRepository = shelterPetRepository;
        }
        
        public async Task HandleAsync(ShelterPetRemoved @event)
        {
            await _shelterPetRepository.DeleteAsync(@event.Id);
        }
    }
}