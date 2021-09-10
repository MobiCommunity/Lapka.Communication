﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Communication.Api.Models.Request
{
    public class CreateAdoptPetMessageRequest
    {
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}