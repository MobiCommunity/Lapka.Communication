using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
{
    public class ShelterPetDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string PetName { get; set; }
        public string Race { get; set; }
        public DateTime BirthDate { get; set; }
        public IEnumerable<string> PhotoPaths { get; set; }
    }
}