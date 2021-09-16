using System;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Mongo.Documents
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