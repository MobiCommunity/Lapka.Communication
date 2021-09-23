using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Communication.Application.Commands.Conversations;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Commands.Handlers.Conversations
{
    public class CreateUserMessageHandler : ICommandHandler<CreateUserMessage>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserConversationRepository _repository;

        public CreateUserMessageHandler(IEventProcessor eventProcessor, IUserConversationRepository repository)
        {
            _eventProcessor = eventProcessor;
            _repository = repository;
        }

        public async Task HandleAsync(CreateUserMessage command)
        {
            UserConversation conversation = await GetAndValidConversationAsync(command);
            
            UserMessage message = new UserMessage(command.UserId, command.Message, false, command.CreatedAt);
            conversation.AddMessage(message);
            
            await _repository.UpdateAsync(conversation);
            await _eventProcessor.ProcessAsync(conversation.Events);
        }

        private async Task<UserConversation> GetAndValidConversationAsync(CreateUserMessage command)
        {
            UserConversation conversation =
                await _repository.GetConversationBetweenUsersAsync(command.UserId, command.ReceiverId);

            if (conversation is null)
            {
                List<Guid> members = new List<Guid> {command.UserId, command.ReceiverId};

                conversation = UserConversation.Create(Guid.NewGuid(), members, new List<UserMessage>());
                await _repository.AddAsync(conversation);
            }

            return conversation;
        }
    }
}