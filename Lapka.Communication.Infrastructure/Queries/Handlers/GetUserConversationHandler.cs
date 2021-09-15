using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure.Documents;

namespace Lapka.Communication.Infrastructure.Queries.Handlers
{
    public class GetUserConversationHandler : IQueryHandler<GetUserConversation, UserDetailedConversationDto>
    {
        private readonly IMongoRepository<UserConversationDocument, Guid> _repository;

        public GetUserConversationHandler(IMongoRepository<UserConversationDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<UserDetailedConversationDto> HandleAsync(GetUserConversation query)
        {
            UserConversationDocument conversation = await GetUserConversationDocumentAsync(query);

            CheckIfUserIsAccessibleOfConversation(query, conversation);

            conversation.Messages = conversation.Messages.OrderByDescending(x => x.CreatedAt).ToList();
            return conversation.AsDetailDto(query.UserId);
        }

        private static void CheckIfUserIsAccessibleOfConversation(GetUserConversation query,
            UserConversationDocument conversation)
        {
            if (conversation.Members.All(x => x != query.UserId))
            {
                throw new UserDoesNotOwnConversationException(query.UserId, query.Id);
            }
        }

        private async Task<UserConversationDocument> GetUserConversationDocumentAsync(GetUserConversation query)
        {
            UserConversationDocument conversation = await _repository.GetAsync(x => x.Id == query.Id);
            if (conversation is null)
            {
                throw new ConversationNotFoundException(query.Id);
            }

            return conversation;
        }
    }
}