using Convey.CQRS.Queries;
using System;
using Lapka.Communication.Application.Dto;

namespace Lapka.Communication.Application.Queries
{
    public class GetValue : IQuery<ValueDto>
    {
        public Guid Id { get; set; }

    }
}
