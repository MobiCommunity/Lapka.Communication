using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class AdoptPetMessageHandler : ICommandHandler<CreateAdoptPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IAdoptPetMessageRepository _repository;

        public AdoptPetMessageHandler(IEventProcessor eventProcessor, IAdoptPetMessageRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(CreateAdoptPetMessage command)
        {
            AdoptPetMessage message = AdoptPetMessage.Create(command.Id, command.UserId, command.ShelterId,
                command.PetId, command.Description, command.FullName, command.PhoneNumber);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }
    }
}