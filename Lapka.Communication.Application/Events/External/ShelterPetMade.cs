using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Lapka.Communication.Application.Events.External
{
    [Message("pets")]
    public class ShelterPetMade : IEvent
    {
        public Guid Id { get; }

        public ShelterPetMade(Guid id)
        {
            Id = id;
        }
    }
}