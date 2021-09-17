using System;
using System.Collections.Generic;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        public Location Location { get; }
        public List<Guid> Owners { get; }

        public Shelter(Guid id, Location location, List<Guid> owners = null)
        {
            Id = new AggregateId(id);
            Location = location;
            Owners = owners ?? new List<Guid>();
        }

        public static Shelter Create(Guid id, Location location)
        {
            Shelter shelter = new Shelter(id, location);
            return shelter;
        }

        public void AddOwner(Guid ownerId)
        {
            Owners.Add(ownerId);
        }
        
        public void RemoveOwner(Guid ownerId)
        {
            Owners.Remove(ownerId);
        }
    }
}