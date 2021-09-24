using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Elasticsearch.Net;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class GetUnreadMessagesCountHandler : IQueryHandler<GetUnreadMessagesCount, long>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUnreadMessagesCountHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<long> HandleAsync(GetUnreadMessagesCount query)
        {
            CountResponse countResponse = await _elasticClient.CountAsync<ShelterMessageDocument>(s => s.Query(q =>
                new QueryContainer(
                    new BoolQuery
                    {
                        Must = new Collection<QueryContainer>
                        {
                            new QueryContainer(new MatchPhraseQuery
                            {
                                Query = query.ShelterId.ToString(),
                                Field = Infer.Field<ShelterMessageDocument>(p => p.ShelterId)
                            }),
                            new QueryContainer(new TermQuery()
                            {
                                Value = false,
                                Field = Infer.Field<ShelterMessageDocument>(p => p.IsRead)
                            }),
                        }
                    })).Index(_elasticSearchOptions.Aliases.ShelterMessages));

            if (countResponse is null)
            {
                throw new ElasticsearchClientException("Could not get shelter messages count");
            }

            return countResponse.Count;
        }
    }
}