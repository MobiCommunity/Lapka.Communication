using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class
        GetUserConversationsHandler : IQueryHandler<GetUserConversations, IEnumerable<UserBasicConversationDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserConversationsHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<UserBasicConversationDto>> HandleAsync(GetUserConversations query)
        {
            List<UserConversationDocument> conversations = await GetUserConversationsAsync();

            return conversations.Select(x => x.AsBasicDto(query.UserId));
        }

        private async Task<List<UserConversationDocument>> GetUserConversationsAsync()
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.UserConversations);

            ISearchResponse<UserConversationDocument> userConversations =
                await _elasticClient.SearchAsync<UserConversationDocument>(searchRequest);

            return userConversations?.Documents.ToList();
        }
    }
}