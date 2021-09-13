using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Communication.Application.Dto;
using Lapka.Communication.Application.Exceptions;
using Lapka.Communication.Application.Queries;
using Lapka.Communication.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Communication.Infrastructure.Queries.Handlers
{
    public class
        GetUserConversationsHandler : IQueryHandler<GetUserConversations, IEnumerable<UserBasicConversationDto>>
    {
        private readonly IMongoRepository<UserConversationDocument, Guid> _repository;

        public GetUserConversationsHandler(IMongoRepository<UserConversationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserBasicConversationDto>> HandleAsync(GetUserConversations query)
        {
            IReadOnlyList<UserConversationDocument> conversations =
                await _repository.FindAsync(x => x.Members.Any(x => x == query.UserId));

            return conversations.Select(x => x.AsDto());
        }
    }
}