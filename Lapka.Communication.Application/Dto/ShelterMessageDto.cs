using System;
using System.Collections.Generic;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Dto
{
    public class ShelterMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public IEnumerable<string> PhotoPaths { get; set; }
    }
}