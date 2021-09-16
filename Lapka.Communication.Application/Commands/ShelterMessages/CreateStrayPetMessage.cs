using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Commands.ShelterMessages
{
    public class CreateStrayPetMessage : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Location Location { get; }
        public List<PhotoFile> Photos { get; }
        public string Description { get; }
        public string ReporterName { get; }
        public string ReporterPhoneNumber { get; }

        public CreateStrayPetMessage(Guid id, Guid userId, Location location, List<PhotoFile> photos,
            string description, string reporterName, string reporterPhoneNumber)
        {
            Id = id;
            UserId = userId;
            Location = location;
            Photos = photos;
            Description = description;
            ReporterName = reporterName;
            ReporterPhoneNumber = reporterPhoneNumber;
        }
    }
}