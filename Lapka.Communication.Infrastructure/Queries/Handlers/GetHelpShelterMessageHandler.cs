using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Infrastructure.Documents;

namespace Lapka.Communication.Infrastructure.Queries.Handlers
{
    public class GetHelpShelterMessageHandler : IQueryHandler<GetHelpShelterMessage, ShelterMessageDto>
    {
        private readonly IMongoRepository<ShelterMessageDocument, Guid> _repository;
        private readonly IGrpcIdentityService _identityService;

        public GetHelpShelterMessageHandler(IMongoRepository<ShelterMessageDocument, Guid> repository,
            IGrpcIdentityService identityService)
        {
            _repository = repository;
            _identityService = identityService;
        }

        public async Task<ShelterMessageDto> HandleAsync(GetHelpShelterMessage query)
        {
            ShelterMessageDocument message = await GetHelpShelterMessageDocumentAsync(query);

            await CheckIfUserIsAccessibleOfMessageAsync(query, message);

            return message.AsDto();
        }

        private async Task CheckIfUserIsAccessibleOfMessageAsync(GetHelpShelterMessage query,
            ShelterMessageDocument message)
        {
            bool isUserOwner = await _identityService.IsUserOwnerOfShelterAsync(message.ShelterId, query.UserId);
            if (!isUserOwner && message.UserId != query.UserId)
            {
                throw new UserDoesNotOwnMessageException(query.MessageId, query.UserId);
            }
        }

        private async Task<ShelterMessageDocument> GetHelpShelterMessageDocumentAsync(GetHelpShelterMessage query)
        {
            ShelterMessageDocument message = await _repository.GetAsync(x => x.Id == query.MessageId);
            if (message is null)
            {
                throw new MessageNotFoundException(query.MessageId);
            }

            return message;
        }
    }
}