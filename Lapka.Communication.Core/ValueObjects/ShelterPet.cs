using System;
using System.Collections.Generic;

namespace Lapka.Communication.Core.ValueObjects
{
    public class ShelterPet
    {
        public Guid Id { get; }
        public string PetName { get; }
        public string Race { get; }
        public DateTime BirthDate { get; }
        public IEnumerable<string> PhotoPaths { get; }

        public ShelterPet(Guid id, string petName, string race, DateTime birthDate, IEnumerable<string> photoPaths)
        {
            Id = id;
            PetName = petName;
            Race = race;
            BirthDate = birthDate;
            PhotoPaths = photoPaths;
        }
    }
}