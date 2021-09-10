using System;

namespace Lapka.Communication.Api.Models.Request
{
    public class CreateAdoptPetMessageRequest
    {
        public Guid PetId { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}