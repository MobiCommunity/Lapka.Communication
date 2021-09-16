using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.Conversations;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers.Conversations
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
            UserConversation conversation = await GetUserConversationAsync(command);

            CheckIfUserOwnConversation(command, conversation);
            
            conversation.Delete();
            
            await _repository.DeleteAsync(conversation);
            await _eventProcessor.ProcessAsync(conversation.Events);
        }

        private static void CheckIfUserOwnConversation(DeleteUserConversation command, UserConversation conversation)
        {
            if (conversation.Members.All(x => x != command.UserId))
            {
                throw new UserDoesNotOwnConversationException(command.UserId, command.ConversationId);
            }
        }

        private async Task<UserConversation> GetUserConversationAsync(DeleteUserConversation command)
        {
            UserConversation conversation = await _repository.GetAsync(command.ConversationId);
            if (conversation is null)
            {
                throw new ConversationNotFoundException(command.ConversationId);
            }

            return conversation;
        }
    }
}