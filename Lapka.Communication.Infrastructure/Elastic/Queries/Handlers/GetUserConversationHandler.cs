using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class GetUserConversationHandler : IQueryHandler<GetUserConversation, UserDetailedConversationDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserConversationHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
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
            GetResponse<UserConversationDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<UserConversationDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.UserConversations));

            UserConversationDocument conversation = response?.Source;
            if (conversation is null)
            {
                throw new ConversationNotFoundException(query.Id);
            }

            return conversation;
        }
    }
}