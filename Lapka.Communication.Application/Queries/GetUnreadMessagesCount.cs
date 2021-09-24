using System;
using Convey.CQRS.Queries;

namespace Lapka.Communication.Application.Queries
{
    public class GetUnreadMessagesCount : IQuery<long>
    {
        public Guid ShelterId { get; set; }
    }
}