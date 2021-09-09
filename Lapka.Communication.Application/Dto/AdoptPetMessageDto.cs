using System;

namespace Lapka.Communication.Application.Dto
{
    public class AdoptPetMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public Guid PetId { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}