using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers
{
    public class DeleteUserConversationHandler : ICommandHandler<DeleteUserConversation>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserConversationRepository _repository;

        public DeleteUserConversationHandler(IEventProcessor eventProcessor, IUserConversationRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(DeleteUserConversation command)
        {
            UserConversation conversation = await _repository.GetAsync(command.ConversationId);
            if (conversation is null)
            {
                throw new ConversationNotFoundException(command.ConversationId);
            }
            
            if (conversation.Members.All(x => x != command.UserId))
            {
                throw new UserDoesNotOwnConversationException(command.UserId, command.ConversationId);
            }
            
            conversation.Delete();
            
            await _repository.DeleteAsync(conversation);
            await _eventProcessor.ProcessAsync(conversation.Events);
        }
    }
}