using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
{
    public class UserConversationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> Members { get; set; }
        public IEnumerable<UserMessageDocument> Messages { get; set; }
    }
}