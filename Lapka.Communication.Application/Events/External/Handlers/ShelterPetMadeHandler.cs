using System.Threading.Tasks;
using Convey.CQRS.Events;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Events.External.Handlers
{
    public class ShelterPetMadeHandler : IEventHandler<ShelterPetMade>
    {
        private readonly IGrpcPetService _grpcPetService;
        private readonly IShelterPetRepository _shelterPetRepository;

        public ShelterPetMadeHandler(IGrpcPetService grpcPetService, IShelterPetRepository shelterPetRepository)
        {
            _grpcPetService = grpcPetService;
            _shelterPetRepository = shelterPetRepository;
        }
        
        public async Task HandleAsync(ShelterPetMade @event)
        {
            ShelterPet pet = await _grpcPetService.GetPetBasicInfoAsync(@event.Id);
            if (pet is null)
            {
                throw new PetDoesNotExistsException(@event.Id);
            }

            await _shelterPetRepository.AddAsync(pet);
        }
    }
}