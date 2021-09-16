using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
{
    public class UserConversationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public List<Guid> Members { get; set; }
        public List<UserMessageDocument> Messages { get; set; }
    }
}