using Lapka.Communication.Application.Dto;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Infrastructure.Documents
{
    public static class Extensions
    {
        public static AdoptPetMessage AsBusiness(this AdoptPetMessageDocument message)
        {
            return new AdoptPetMessage(message.Id, message.UserId, message.ShelterId, message.PetId, message.Description,
                message.FullName, message.PhoneNumber, message.CreatedAt);
        }
        
        public static AdoptPetMessageDocument AsDocument(this AdoptPetMessage message)
        {
            return new AdoptPetMessageDocument
            {
                Id = message.Id.Value,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PetId = message.PetId,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }
        
        public static AdoptPetMessageDto AsDto(this AdoptPetMessageDocument message)
        {
            return new AdoptPetMessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ShelterId = message.ShelterId,
                PetId = message.PetId,
                Description = message.Description,
                FullName = message.FullName,
                PhoneNumber = message.PhoneNumber,
                CreatedAt = message.CreatedAt
            };
        }
    }
}