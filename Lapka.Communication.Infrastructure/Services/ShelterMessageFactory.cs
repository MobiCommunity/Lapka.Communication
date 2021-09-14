using System;
using System.Text;
using Lapka.Communication.Application.Commands;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Infrastructure.Services
{
    public class ShelterMessageFactory : IShelterMessageFactory
    {
        public ShelterMessage CreateAdoptPetMessage(CreateAdoptPetMessage message, Guid shelterId)
        {
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.FullName} chce zaadoptować zwierzaka {message.PetId}. ");
            msgDescription.Append($"Z użytkownikiem można się skontakować dzwoniąc pod: {message.PhoneNumber}. ");
            msgDescription.Append($"Użytkownik do adopcji dołaczył wiadomość o treści: {message.Description}");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, shelterId,
                "Adopcja zwierzaka", msgDescription.ToString(), message.FullName, message.PhoneNumber,
                DateTime.Now);

            return shelterMessage;
        }

        public ShelterMessage CreateHelpShelterMessage(CreateHelpShelterMessage message)
        {
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.FullName} chce pomóc schronisku. ");
            msgDescription.Append($"Jako oferowana forme pomocy wybrał: {message.HelpType} ");
            msgDescription.Append($"Z użytkownikiem można się skontakować dzwoniąc pod: {message.PhoneNumber}. ");
            msgDescription.Append($"Użytkownik do chęci pomocy dołaczył wiadomość o treści: {message.Description}");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, message.ShelterId,
                "Adopcja zwierzaka", msgDescription.ToString(), message.FullName, message.PhoneNumber,
                DateTime.Now);

            return shelterMessage;
        }

        public ShelterMessage CreateStrayPetMessage(CreateStrayPetMessage message, Guid shelterId)
        {
            StringBuilder msgDescription = new StringBuilder();
            msgDescription.Append($"Użytkownik {message.ReporterName} chce zgłasza błakającego sie zwierzaka. ");
            msgDescription.Append($"Z użytkownikiem można się skontakować dzwoniąc pod: {message.ReporterPhoneNumber}. ");
            msgDescription.Append($"Użytkownik do zgłoszenia dołaczył wiadomość o treści: {message.Description}");

            ShelterMessage shelterMessage = ShelterMessage.Create(message.Id, message.UserId, shelterId,
                "Błąkający się zwierzak", msgDescription.ToString(), message.ReporterName, message.ReporterPhoneNumber,
                DateTime.Now);

            return shelterMessage;
        }
    }
}