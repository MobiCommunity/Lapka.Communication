using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetStrayPetMessages : IQuery<IEnumerable<StrayPetMessageDto>>
    {
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
    }
}