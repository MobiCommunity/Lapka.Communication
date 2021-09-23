using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Application.Services.Repositories;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Infrastructure.Elastic.Options;
using Lapka.Communication.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Communication.Infrastructure.Elastic.Queries.Handlers
{
    public class GetShelterMessageHandler : IQueryHandler<GetShelterMessage, ShelterMessageDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;
        private readonly IShelterRepository _shelterRepository;

        public GetShelterMessageHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions,
            IShelterRepository shelterRepository)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
            _shelterRepository = shelterRepository;
        }

        public async Task<ShelterMessageDto> HandleAsync(GetShelterMessage query)
        {
            ShelterMessageDocument message = await GetHelpShelterMessageDocumentAsync(query);

            await CheckIfUserIsAccessibleOfMessageAsync(query, message);

            return message.AsDto();
        }

        private async Task CheckIfUserIsAccessibleOfMessageAsync(GetShelterMessage query,
            ShelterMessageDocument message)
        {
            Shelter shelter = await _shelterRepository.GetAsync(message.ShelterId);
            if (shelter.Owners.Any(x => x != query.UserId) && message.UserId != query.UserId)
            {
                throw new UserDoesNotOwnMessageException(query.MessageId, query.UserId);
            }
        }

        private async Task<ShelterMessageDocument> GetHelpShelterMessageDocumentAsync(GetShelterMessage query)
        {
            GetResponse<ShelterMessageDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<ShelterMessageDocument>(new Id(query.MessageId.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.ShelterMessages));

            ShelterMessageDocument message = response?.Source;
            if (message is null)
            {
                throw new MessageNotFoundException(query.MessageId);
            }

            return message;
        }
    }
}