using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Services
{
    public class ElasticSearchSeeder : IHostedService
    {
        private readonly IMongoRepository<UserConversationDocument, Guid> _userConversationRepository;
        private readonly IMongoRepository<ShelterMessageDocument, Guid> _shelterMessageRepository;
        private readonly ElasticSearchOptions _elasticOptions;
        private readonly ILogger<ElasticSearchSeeder> _logger;
        private readonly IElasticClient _client;

        public ElasticSearchSeeder(ILogger<ElasticSearchSeeder> logger,
            IMongoRepository<ShelterMessageDocument, Guid> shelterMessageRepository, IElasticClient client,
            IMongoRepository<UserConversationDocument, Guid> userConversationRepository,
            ElasticSearchOptions elasticOptions)
        {
            _userConversationRepository = userConversationRepository;
            _shelterMessageRepository = shelterMessageRepository;
            _elasticOptions = elasticOptions;
            _logger = logger;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SeedShelterMessagesAsync();
            await SeedUserConversationAsync();
        }

        private async Task SeedUserConversationAsync()
        {
            IReadOnlyList<UserConversationDocument> photos = await _userConversationRepository.FindAsync(_ => true);

            BulkAllObservable<UserConversationDocument> bulkPhotos =
                _client.BulkAll(photos, b => b.Index(_elasticOptions.Aliases.ShelterMessages));

            bulkPhotos.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("User conversations indexed"));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        private async Task SeedShelterMessagesAsync()
        {
            IReadOnlyList<ShelterMessageDocument> photos = await _shelterMessageRepository.FindAsync(_ => true);

            BulkAllObservable<ShelterMessageDocument> bulkPhotos =
                _client.BulkAll(photos, b => b.Index(_elasticOptions.Aliases.ShelterMessages));

            bulkPhotos.Wait(TimeSpan.FromMinutes(5), x => _logger.LogInformation("Shelter messages indexed"));
        }
    }
}