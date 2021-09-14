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
        public static ShelterMessage ArrangeShelterMessage(AggregateId id = null, Guid? userId = null,
            Guid? shelterId = null, string title = null, string description = null, string fullName = null,
            string phoneNumber = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            string validTitle = title ?? "This is valid Title";
            string validDescription = description ??
                                      "I want to adopt this pet because this pet is the coolest pet i have ever seen.";
            string validFullName = fullName ?? "Mikolaj mikołajczyk";
            string validPhoneNumber = phoneNumber ?? "123123123";

            ShelterMessage message = ShelterMessage.Create(validId.Value, validUserId, validShelterId, validTitle,
                validDescription, validFullName, validPhoneNumber, DateTime.Now);

            return message;
        }

        // public static ShelterMessage ArrangeShelterMessage(AggregateId id = null, Guid? userId = null,
        //     Guid? shelterId = null, string title = null, MessageDescription description = null, FullName fullName = null,
        //     PhoneNumber phoneNumber = null)
        // {
        //     AggregateId validId = id ?? new AggregateId();
        //     Guid validUserId = userId ?? Guid.NewGuid();
        //     Guid validShelterId = shelterId ?? Guid.NewGuid();
        //     string validTitle = title ?? "This is valid Title";
        //     MessageDescription validDescription = description ??
        //                                           new MessageDescription(
        //                                               "I want to adopt this pet because this pet is the coolest pet i have ever seen.");
        //     FullName validFullName = fullName ?? new FullName("Mikolaj mikołajczyk");
        //     PhoneNumber validPhoneNumber = phoneNumber ?? new PhoneNumber("123123123");
        //
        //     ShelterMessage message = new ShelterMessage(validId.Value, validUserId, validShelterId, validTitle,
        //         validDescription, validFullName, validPhoneNumber, DateTime.Now);
        //
        //     return message;
        // }

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