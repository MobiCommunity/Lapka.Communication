using System.Linq;
using System.Threading.Tasks;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Application.Services.Elastic;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Services
{
    public class UserConversationElasticsearchUpdater : IUserConversationElasticsearchUpdater
    {
        private readonly ILogger<UserConversationElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public UserConversationElasticsearchUpdater(ILogger<UserConversationElasticsearchUpdater> logger,
            IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(UserConversation conversation)
        {
            IndexResponse response = await _elasticClient.IndexAsync(conversation.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.UserConversations));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" user conversation {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }

        public async Task DeleteDataAsync(UserConversation conversation)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<UserConversation>(conversation.Id.Value,
                x => x.Index(_elasticSearchOptions.Aliases.UserConversations));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" user conversation {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}