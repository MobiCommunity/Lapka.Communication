using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Commands.ShelterMessages;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Infrastructure.Services
{
    public class ShelterMessageFactory : IShelterMessageFactory
    {
        private readonly IShelterPetRepository _shelterPetRepository;

        public ShelterMessageFactory(IShelterPetRepository shelterPetRepository)
        {
            _shelterPetRepository = shelterPetRepository;
        }
        
        public async Task<ShelterMessage> CreateFromAdoptPetMessageAsync(CreateAdoptPetMessage message, Guid shelterId)
        {
            ShelterPet pet = await _shelterPetRepository.GetAsync(message.PetId);
            
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.FullName} chce zaadoptować zwierzaka {pet.PetName}. ");
            msgDescription.Append($"Zwierzak jest urodzony dnia {pet.BirthDate} o rasie {pet.Race}. ");
            msgDescription.Append($"Z użytkownikiem można się skontakować dzwoniąc pod: {message.PhoneNumber}. ");
            msgDescription.Append($"Użytkownik do adopcji dołaczył wiadomość o treści: {message.Description}");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, shelterId, false,
                "Adopcja zwierzaka", new MessageDescription(msgDescription.ToString()), new FullName(message.FullName),
                new PhoneNumber(message.PhoneNumber), DateTime.UtcNow);

            return shelterMessage;
        }

        public async Task<ShelterMessage> CreateFromStrayPetMessageAsync(CreateStrayPetMessage message, Guid shelterId,
            IEnumerable<string> photoPath)
        {
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.ReporterName} zgłasza błakającego sie zwierzaka. ");
            msgDescription.Append(
                $"Z użytkownikiem można się skontakować dzwoniąc pod: {message.ReporterPhoneNumber}. ");
            msgDescription.Append($"Użytkownik do zgłoszenia dołaczył wiadomość o treści: {message.Description}. ");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, shelterId, false,
                "Błąkający się zwierzak", new MessageDescription(msgDescription.ToString()),
                new FullName(message.ReporterName), new PhoneNumber(message.ReporterPhoneNumber), DateTime.UtcNow, photoPath);

            return shelterMessage;
        }
        
        public ShelterMessage CreateFromHelpShelterMessage(CreateHelpShelterMessage message)
        {
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.FullName} chce pomóc schronisku. ");
            msgDescription.Append($"Jako oferowana forme pomocy wybrał: {message.HelpType} ");
            msgDescription.Append($"Z użytkownikiem można się skontakować dzwoniąc pod: {message.PhoneNumber}. ");
            msgDescription.Append($"Użytkownik do chęci pomocy dołaczył wiadomość o treści: {message.Description}");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, message.ShelterId, false,
                "Adopcja zwierzaka", new MessageDescription(msgDescription.ToString()), new FullName(message.FullName),
                new PhoneNumber(message.PhoneNumber), DateTime.UtcNow);

            return shelterMessage;
        }
    }
}