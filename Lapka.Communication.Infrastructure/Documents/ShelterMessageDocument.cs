using System;
using Convey.Types;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Infrastructure.Documents
{
    public class ShelterMessageDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}