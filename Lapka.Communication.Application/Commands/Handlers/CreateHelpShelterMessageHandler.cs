using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class CreateHelpShelterMessageHandler : ICommandHandler<CreateHelpShelterMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IHelpShelterMessageRepository _repository;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public CreateHelpShelterMessageHandler(IEventProcessor eventProcessor, IHelpShelterMessageRepository repository,
            IGrpcIdentityService grpcIdentityService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(CreateHelpShelterMessage command)
        {
            await ValidShelterExistenceAsync(command);
            
            HelpShelterMessage message = HelpShelterMessage.Create(command.Id, command.UserId, command.ShelterId,
                command.HelpType, command.Description, command.FullName, command.PhoneNumber);

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