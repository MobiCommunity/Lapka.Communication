using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class CreateHelpShelterMessageHandler : ICommandHandler<CreateHelpShelterMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterMessageRepository _repository;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IShelterMessageFactory _messageFactory;

        public CreateHelpShelterMessageHandler(IEventProcessor eventProcessor, IShelterMessageRepository repository,
            IGrpcIdentityService grpcIdentityService, IShelterMessageFactory messageFactory)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcIdentityService = grpcIdentityService;
            _messageFactory = messageFactory;
        }

        public async Task HandleAsync(CreateHelpShelterMessage command)
        {
            await ValidShelterExistenceAsync(command);
            
            ShelterMessage message = _messageFactory.CreateHelpShelterMessage(command);

            await _repository.AddAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task ValidShelterExistenceAsync(CreateHelpShelterMessage command)
        {
            try
            {
                if (!await _grpcIdentityService.DoesShelterExists(command.ShelterId))
                {
                    throw new ShelterDoesNotExistsException(command.ShelterId);
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ShelterDoesNotExistsException))
                {
                    throw new ShelterDoesNotExistsException(command.ShelterId);
                }
                throw new CannotRequestIdentityMicroserviceException(ex);
            }
        }
    }
}