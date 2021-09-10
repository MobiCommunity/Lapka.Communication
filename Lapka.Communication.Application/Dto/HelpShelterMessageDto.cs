﻿using System;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Application.Dto
{
    public class HelpShelterMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public HelpType HelpType { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}