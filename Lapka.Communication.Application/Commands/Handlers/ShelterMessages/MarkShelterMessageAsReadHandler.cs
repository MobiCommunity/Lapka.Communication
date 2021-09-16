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
    public class MarkShelterMessageAsReadHandler : ICommandHandler<MarkShelterMessageAsRead>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterMessageRepository _repository;
        private readonly IGrpcIdentityService _grpcIdentityService;

        public MarkShelterMessageAsReadHandler(IEventProcessor eventProcessor, IShelterMessageRepository repository,
            IGrpcIdentityService grpcIdentityService)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _grpcIdentityService = grpcIdentityService;
        }

        public async Task HandleAsync(MarkShelterMessageAsRead command)
        {
            ShelterMessage message = await GetShelterMessageAsync(command);
            await ValidIfUserIsOwnerOfShelter(command, message);
            
            message.MarkAsRead();
            
            await _repository.UpdateAsync(message);
            await _eventProcessor.ProcessAsync(message.Events);
        }

        private async Task ValidIfUserIsOwnerOfShelter(MarkShelterMessageAsRead command, ShelterMessage message)
        {
            bool isUserOwner = await _grpcIdentityService.IsUserOwnerOfShelterAsync(message.ShelterId, command.UserId);
            if (!isUserOwner)
            {
                throw new UserNotOwnerOfShelterException(message.ShelterId, command.UserId);
            }
        }

        private async Task<ShelterMessage> GetShelterMessageAsync(MarkShelterMessageAsRead command)
        {
            ShelterMessage message = await _repository.GetAsync(command.MessageId);
            if (message is null)
            {
                throw new MessageNotFoundException(command.MessageId);
            }

            return message;
        }
    }
}