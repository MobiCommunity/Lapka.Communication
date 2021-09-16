using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.Conversations;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Commands.Handlers.Conversations
{
    public class MarkUserMessageAsReadHandler : ICommandHandler<MarkUserMessageAsRead>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserConversationRepository _repository;

        public MarkUserMessageAsReadHandler(IEventProcessor eventProcessor, IUserConversationRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(MarkUserMessageAsRead command)
        {
            UserConversation conversation = await GetConversationAsync(command);
            ValidIfUserIsAccessibleOfConversation(command, conversation);
            
            MarkUnreadMessageAsRead(command, conversation);

            await _repository.UpdateAsync(conversation);
            await _eventProcessor.ProcessAsync(conversation.Events);
        }

        private static void ValidIfUserIsAccessibleOfConversation(MarkUserMessageAsRead command, UserConversation conversation)
        {
            if (conversation.Members.All(x => x != command.UserId))
            {
                throw new UserDoesNotOwnConversationException(command.UserId, command.ConversationId);
            }
        }

        private static void MarkUnreadMessageAsRead(MarkUserMessageAsRead command, UserConversation conversation)
        {
            IEnumerable<UserMessage> messages =
                conversation.Messages.Where(x => x.SenderUserId != command.UserId && x.IsReadByReceiver == false);
            foreach (UserMessage message in messages)
            {
                message.MarkAsRead();
            }
        }

        private async Task<UserConversation> GetConversationAsync(MarkUserMessageAsRead command)
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