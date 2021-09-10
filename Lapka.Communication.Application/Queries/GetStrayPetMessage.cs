using System;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetStrayPetMessage : IQuery<StrayPetMessageDto>
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
    }
}