using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Communication.Infrastructure.Queries.Handlers
{
    public class GetUserConversationsHandler : IQueryHandler<GetUserConversations, List<UserBasicConversationDto>>
    {
        private readonly IMongoRepository<UserConversationDocument, Guid> _repository;

        public GetUserConversationsHandler(IMongoRepository<UserConversationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<List<UserBasicConversationDto>> HandleAsync(GetUserConversations query)
        {
            List<UserBasicConversationDto> userConversations = new List<UserBasicConversationDto>();

            IReadOnlyList<UserConversationDocument> conversations =
                await _repository.FindAsync(x => x.Members.Any(x => x == query.UserId));

            foreach (UserConversationDocument conversation in conversations)
            {
                UserMessageDocument lastMessage =
                    conversation.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                
                userConversations.Add(new UserBasicConversationDto
                {
                    ConversationId = conversation.Id,
                    LastMessage = lastMessage.Message,
                    LastMessageCreation = lastMessage.CreatedAt
                });
            }

            return userConversations;
        }
    }
}