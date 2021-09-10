using System;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetUserConversation : IQuery<UserDetailedConversationDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}