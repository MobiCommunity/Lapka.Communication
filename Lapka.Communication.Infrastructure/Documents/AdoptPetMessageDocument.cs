using System;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Documents
{
    public class AdoptPetMessageDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid ShelterId { get; set; }
        public Guid PetId { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}