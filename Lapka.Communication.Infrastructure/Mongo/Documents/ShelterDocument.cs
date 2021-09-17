using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
{
    public class ShelterDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public LocationDocument Location { get; set; }
        public List<Guid> Owners { get; set; }
    }
}