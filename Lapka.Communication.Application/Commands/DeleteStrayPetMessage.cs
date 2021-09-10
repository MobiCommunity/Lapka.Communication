using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Communication.Core.ValueObjects;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Application.Commands
{
    public class DeleteStrayPetMessage : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public DeleteStrayPetMessage(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}