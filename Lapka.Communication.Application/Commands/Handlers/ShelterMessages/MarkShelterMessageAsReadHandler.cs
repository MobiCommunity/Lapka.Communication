using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers.ShelterMessages
{
    public class MarkShelterMessageAsReadHandler : ICommandHandler<MarkShelterMessageAsRead>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterMessageRepository _repository;
        private readonly IShelterRepository _shelterRepository;

        public MarkShelterMessageAsReadHandler(IEventProcessor eventProcessor, IShelterMessageRepository repository,
            IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
            _shelterRepository = shelterRepository;
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
            Shelter shelter = await _shelterRepository.GetAsync(message.ShelterId);
            if (shelter.Owners.Any(x => x != command.UserId))
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