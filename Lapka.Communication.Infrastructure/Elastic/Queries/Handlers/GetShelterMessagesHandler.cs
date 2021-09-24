using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterMessagesHandler : IQueryHandler<GetShelterMessages, IEnumerable<ShelterMessageDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;
        private readonly IShelterRepository _shelterRepository;

        public GetShelterMessagesHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions,
            IShelterRepository shelterRepository)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
            _shelterRepository = shelterRepository;
        }

        public async Task<IEnumerable<ShelterMessageDto>> HandleAsync(GetShelterMessages query)
        {
            await CheckIfUserIsOwnerOfShelterAsync(query);

            List<ShelterMessageDocument> messages = await GetShelterMessageDocumentsAsync(query);

            return messages.Select(x => x.AsDto());
        }

        private async Task<List<ShelterMessageDocument>> GetShelterMessageDocumentsAsync(GetShelterMessages query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterMessages)
            {
                Query = new MatchQuery
                {
                    Query = query.ShelterId.ToString(),
                    Field = Infer.Field<ShelterMessageDocument>(p => p.ShelterId)
                }
            };
            ISearchResponse<ShelterMessageDocument>
                shelters = await _elasticClient.SearchAsync<ShelterMessageDocument>(searchRequest);

            List<ShelterMessageDocument> messages = shelters?.Documents.ToList();
            return messages;
        }

        private async Task CheckIfUserIsOwnerOfShelterAsync(GetShelterMessages query)
        {
            Shelter shelter = await _shelterRepository.GetAsync(query.ShelterId);
            if (!shelter.Owners.Contains(query.UserId))
            {
                throw new UserNotOwnerOfShelterException(query.ShelterId, query.UserId);
            }
        }
    }
}