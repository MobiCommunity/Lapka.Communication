using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetShelterMessagesHandler : IQueryHandler<GetShelterMessages, IEnumerable<ShelterMessageDto>>
    {
        private readonly IMongoRepository<ShelterMessageDocument, Guid> _repository;
        private readonly IGrpcIdentityService _identityService;

        public GetShelterMessagesHandler(IMongoRepository<ShelterMessageDocument, Guid> repository,
            IGrpcIdentityService identityService)
        {
            _repository = repository;
            _identityService = identityService;
        }

        public async Task<IEnumerable<ShelterMessageDto>> HandleAsync(GetShelterMessages query)
        {
            await CheckIfUserIsOwnerOfShelterAsync(query);

            IReadOnlyList<ShelterMessageDocument> messages =
                await _repository.FindAsync(x => x.ShelterId == query.ShelterId);

            return messages.Select(x => x.AsDto());
        }

        private async Task CheckIfUserIsOwnerOfShelterAsync(GetShelterMessages query)
        {
            bool isUserOwner = await _identityService.IsUserOwnerOfShelterAsync(query.ShelterId, query.UserId);
            if (!isUserOwner)
            {
                throw new UserNotOwnerOfShelterException(query.ShelterId, query.UserId);
            }
        }
    }
}