using System;
using System.IO;
using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;
using File = Lapka.Communication.Core.ValueObjects.File;

namespace Lapka.Communication.Unit.Tests
{
    public static class Extensions
    {
        public static ShelterMessage ArrangeShelterMessage(AggregateId id = null, Guid? userId = null,
            Guid? shelterId = null, string title = null, MessageDescription description = null,
            FullName fullName = null,
            PhoneNumber phoneNumber = null)
        {
            AggregateId validId = id ?? new AggregateId();
            Guid validUserId = userId ?? Guid.NewGuid();
            Guid validShelterId = shelterId ?? Guid.NewGuid();
            string validTitle = title ?? "This is valid Title";
            MessageDescription validDescription = description ?? new MessageDescription(
                "I want to adopt this pet because this pet is the coolest pet i have ever seen.");
            FullName validFullName = fullName ?? new FullName("Mikolaj mikołajczyk");
            PhoneNumber validPhoneNumber = phoneNumber ?? new PhoneNumber("123123123");

            ShelterMessage message = ShelterMessage.Create(validId.Value, validUserId, validShelterId, false,
                validTitle,
                validDescription, validFullName, validPhoneNumber, DateTime.UtcNow);

            return message;
        }

        public static Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        public static File ArrangePhotoFile(string name = null, Stream stream = null, string contentType = null)
        {
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            File file = new File(validName, validStream, validContentType);

            return file;
        }
    }
}