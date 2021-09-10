using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class AdoptPetMessageHandler : ICommandHandler<CreateAdoptPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IAdoptPetMessageRepository _repository;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IGrpcPetService _grpcPetService;

        public AdoptPetMessageHandler(IEventProcessor eventProcessor, IAdoptPetMessageRepository repository,
            IGrpcIdentityService grpcIdentityService, IGrpcPetService grpcPetService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcIdentityService = grpcIdentityService;
            _grpcPetService = grpcPetService;
        }

        public async Task HandleAsync(CreateAdoptPetMessage command)
        {
            if (!await _grpcIdentityService.DoesShelterExistsAsync(command.ShelterId))
            {
                throw new ShelterDoesNotExistsException(command.ShelterId);
            }
            
            if (!await _grpcPetService.DoesPetExists(command.PetId))
            {
                throw new PetDoesNotExistsException(command.ShelterId);
            }
            
            AdoptPetMessage message = AdoptPetMessage.Create(command.Id, command.UserId, command.ShelterId,
                command.PetId, command.Description, command.FullName, command.PhoneNumber);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }
    }
}