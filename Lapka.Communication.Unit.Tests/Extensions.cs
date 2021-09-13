using System;
using System.Collections.Generic;
using System.IO;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Unit.Tests
{
    public static class Extensions
    {
        public static AdoptPetMessage ArrangeAdoptPetMessage(AggregateId id = null, Guid? userId = null,
            Guid? shelterId = null, Guid? petId = null, MessageDescription description = null, FullName fullName = null,
            PhoneNumber phoneNumber = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            Guid validPetId = petId ?? Guid.NewGuid();
            MessageDescription validDescription = description ??
                                                  new MessageDescription(
                                                      "I want to adopt this pet because this pet is the coolest pet i have ever seen.");
            FullName validFullName = fullName ?? new FullName("Mikolaj mikołajczyk");
            PhoneNumber validPhoneNumber = phoneNumber ?? new PhoneNumber("123123123");

            AdoptPetMessage message = new AdoptPetMessage(validId.Value, validUserId, validShelterId, validPetId,
                validDescription, validFullName, validPhoneNumber, DateTime.Now);

            return message;
        }

        public static HelpShelterMessage ArrangeHelpShelterMessage(AggregateId id = null, Guid? userId = null,
            Guid? shelterId = null, HelpType? helpType = null, MessageDescription description = null,
            FullName fullName = null,
            PhoneNumber phoneNumber = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            HelpType validHelpType = helpType ?? HelpType.Walk;
            MessageDescription validDescription = description ??
                                                  new MessageDescription(
                                                      "I want to adopt this pet because this pet is the coolest pet i have ever seen.");
            FullName validFullName = fullName ?? new FullName("Mikolaj mikołajczyk");
            PhoneNumber validPhoneNumber = phoneNumber ?? new PhoneNumber("123123123");

            HelpShelterMessage message = new HelpShelterMessage(validId.Value, validUserId, validShelterId, validHelpType,
                validDescription, validFullName, validPhoneNumber, DateTime.Now);

            return message;
        }
        
        public static StrayPetMessage ArrangeStrayPetMessage(AggregateId id = null, Guid? userId = null,
            Guid? shelterId = null, List<Guid> photoIds = null, MessageDescription description = null,
            FullName fullName = null,
            PhoneNumber phoneNumber = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            List<Guid> validPhotoIds = photoIds ?? new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            MessageDescription validDescription = description ??
                                                  new MessageDescription(
                                                      "I want to adopt this pet because this pet is the coolest pet i have ever seen.");
            FullName validFullName = fullName ?? new FullName("Mikolaj mikołajczyk");
            PhoneNumber validPhoneNumber = phoneNumber ?? new PhoneNumber("123123123");

            StrayPetMessage message = new StrayPetMessage(validId.Value, validUserId, validShelterId, validPhotoIds,
                validDescription, validFullName, validPhoneNumber, DateTime.Now);

            return message;
        }

        public static Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        public static PhotoFile ArrangePhotoFile(Guid? id = null, string name = null, Stream stream = null,
            string contentType = null)
        {
            Guid validId = id ?? Guid.NewGuid();
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            PhotoFile file = new PhotoFile(validId, validName, validStream, validContentType);

            return file;
        }
    }
}