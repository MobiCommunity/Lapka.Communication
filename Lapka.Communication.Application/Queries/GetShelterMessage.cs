using System;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetShelterMessage : IQuery<ShelterMessageDto>
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
    }
}