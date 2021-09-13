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
    public class GetHelpShelterMessageHandler : IQueryHandler<GetHelpShelterMessage, HelpShelterMessageDto>
    {
        private readonly IMongoRepository<HelpShelterMessageDocument, Guid> _repository;
        private readonly IGrpcIdentityService _identityService;

        public GetHelpShelterMessageHandler(IMongoRepository<HelpShelterMessageDocument, Guid> repository,
            IGrpcIdentityService identityService)
        {
            _repository = repository;
            _identityService = identityService;
        }

        public async Task<HelpShelterMessageDto> HandleAsync(GetHelpShelterMessage query)
        {
            HelpShelterMessageDocument message = await GetHelpShelterMessageDocumentAsync(query);

            await CheckIfUserIsAccessibleOfMessageAsync(query, message);

            return message.AsDto();
        }

        private async Task CheckIfUserIsAccessibleOfMessageAsync(GetHelpShelterMessage query,
            HelpShelterMessageDocument message)
        {
            bool isUserOwner = await _identityService.IsUserOwnerOfShelterAsync(message.ShelterId, query.UserId);
            if (!isUserOwner && message.UserId != query.UserId)
            {
                throw new UserDoesNotOwnMessageException(query.MessageId, query.UserId);
            }
        }

        private async Task<HelpShelterMessageDocument> GetHelpShelterMessageDocumentAsync(GetHelpShelterMessage query)
        {
            HelpShelterMessageDocument message = await _repository.GetAsync(x => x.Id == query.MessageId);
            if (message is null)
            {
                throw new MessageNotFoundException(query.MessageId);
            }

            return message;
        }
    }
}