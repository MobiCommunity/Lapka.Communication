using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Application.Services.Grpc;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterMessagesHandler : IQueryHandler<GetShelterMessages, IEnumerable<ShelterMessageDto>>
    {
        private readonly ILogger<GetShelterMessagesHandler> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;
        private readonly IGrpcIdentityService _identityService;

        public GetShelterMessagesHandler(ILogger<GetShelterMessagesHandler> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions, IGrpcIdentityService identityService)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
            _identityService = identityService;
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
            bool isUserOwner = false;
            try
            {
                isUserOwner = await _identityService.IsUserOwnerOfShelterAsync(query.ShelterId, query.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to connect with identity microservice.");
                isUserOwner = false;
            }

            if (!isUserOwner)
            {
                throw new UserNotOwnerOfShelterException(query.ShelterId, query.UserId);
            }
        }
    }
}