﻿using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Communication.Infrastructure.Documents
{
    public class StrayPetMessageDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public List<Guid> PhotoIds { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}