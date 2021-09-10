using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetHelpShelterMessages : IQuery<IEnumerable<HelpShelterMessageDto>>
    {
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
    }
}