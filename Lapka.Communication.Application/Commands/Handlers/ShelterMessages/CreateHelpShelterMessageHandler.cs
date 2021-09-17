using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers.ShelterMessages
{
    public class CreateHelpShelterMessageHandler : ICommandHandler<CreateHelpShelterMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterMessageFactory _messageFactory;
        private readonly IShelterRepository _shelterRepository;

        public CreateHelpShelterMessageHandler(IEventProcessor eventProcessor, IShelterMessageRepository repository,
            IShelterMessageFactory messageFactory, IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _messageFactory = messageFactory;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(CreateHelpShelterMessage command)
        {
            await ValidShelterExistenceAsync(command);
            
            ShelterMessage message = _messageFactory.CreateFromHelpShelterMessage(command);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task ValidShelterExistenceAsync(CreateHelpShelterMessage command)
        {

            Shelter shelter = await _shelterRepository.GetAsync(command.ShelterId);
            if (shelter is null)
            {
                throw new ShelterDoesNotExistsException(command.ShelterId);
            }
        }
    }
}