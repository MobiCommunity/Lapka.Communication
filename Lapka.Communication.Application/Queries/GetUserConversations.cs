using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetUserConversations : IQuery<IEnumerable<UserBasicConversationDto>>
    {
        public Guid UserId { get; set; }
    }
}