using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class DeleteAdoptPetMessageHandler : ICommandHandler<DeleteAdoptPetMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcIdentityService _grpcIdentityService;
        private readonly IAdoptPetMessageRepository _repository;


        public DeleteAdoptPetMessageHandler(IEventProcessor eventProcessor, IGrpcIdentityService grpcIdentityService,
            IAdoptPetMessageRepository repository)
        {
            _eventProcessor = eventProcessor;
            _grpcIdentityService = grpcIdentityService;
            _repository = repository;
        }

        public async Task HandleAsync(DeleteAdoptPetMessage command)
        {
            AdoptPetMessage message = await GetAdoptMessageAsync(command);

            await ValidIfUserCanDeleteAsync(command, message);
            
            message.Delete();

            await _repository.DeleteAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task<AdoptPetMessage> GetAdoptMessageAsync(DeleteAdoptPetMessage command)
        {
            AdoptPetMessage message = await _repository.GetAsync(command.Id);
            if (message is null)
            {
                throw new MessageNotFoundException(command.Id);
            }

            return message;
        }

        private async Task ValidIfUserCanDeleteAsync(DeleteAdoptPetMessage command, AdoptPetMessage message)
        {
            bool isUserOwner = await _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, command.UserId);
            if (!isUserOwner && message.UserId != command.UserId)
            {
                throw new UserDoesNotOwnMessageException(command.UserId, message.Id.Value);
            }
        }
    }
}