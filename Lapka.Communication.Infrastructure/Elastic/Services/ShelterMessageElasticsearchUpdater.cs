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
    public class ShelterMessageElasticsearchUpdater : IShelterMessageElasticsearchUpdater
    {
        private readonly ILogger<ShelterMessageElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public ShelterMessageElasticsearchUpdater(ILogger<ShelterMessageElasticsearchUpdater> logger,
            IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task InsertAndUpdateDataAsync(ShelterMessage message)
        {
            IndexResponse response = await _elasticClient.IndexAsync(message.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.ShelterMessages));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" shelter message {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }

        public async Task DeleteDataAsync(ShelterMessage message)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<ShelterMessage>(message.Id.Value,
                x => x.Index(_elasticSearchOptions.Aliases.ShelterMessages));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" shelter message {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}