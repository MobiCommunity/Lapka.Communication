using System;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetHelpShelterMessage : IQuery<HelpShelterMessageDto>
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
    }
}