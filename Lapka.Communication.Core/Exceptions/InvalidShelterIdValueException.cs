using System;
using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidShelterIdValueException : DomainException
    {
        public Guid ShelterId { get; }
        public InvalidShelterIdValueException(Guid shelterId) : base($"Invalid shelter id: {shelterId}")
        {
            ShelterId = shelterId;
        }

        public override string Code => "invalid_shelter_id";
    }
}