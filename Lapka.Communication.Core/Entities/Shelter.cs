using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Communication.Core.ValueObjects.Locations;

namespace Lapka.Communication.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        private ISet<Guid> _owners = new HashSet<Guid>();
        public Location Location { get; }

        public IEnumerable<Guid> Owners
        {
            get => _owners;
            private set => _owners = new HashSet<Guid>(value);
        }

        public Shelter(Guid id, Location location, IEnumerable<Guid> owners = null)
        {
            Id = new AggregateId(id);
            Location = location;
            Owners = owners ?? Enumerable.Empty<Guid>();
        }

        public static Shelter Create(Guid id, Location location)
        {
            Shelter shelter = new Shelter(id, location);
            return shelter;
        }

        public void AddOwner(Guid ownerId)
        {
            _owners.Add(ownerId);
        }
        
        public void RemoveOwner(Guid ownerId)
        {
            _owners.Remove(ownerId);
        }
    }
}