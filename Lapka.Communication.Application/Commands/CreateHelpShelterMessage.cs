using System;
using Convey.CQRS.Commands;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Commands
{
    public class CreateHelpShelterMessage : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public HelpType HelpType { get; }
        public string Description { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }

        public CreateHelpShelterMessage(Guid id, Guid userId, Guid shelterId, HelpType helpType, string description,
            string fullName, string phoneNumber)
        {
            Id = id;
            UserId = userId;
            ShelterId = shelterId;
            HelpType = helpType;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
    }
}