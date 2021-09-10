using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Api.Models.Request
{
    public class CreateHelpShelterMessageRequest
    {
        [Required]
        public Guid ShelterId { get; set; }
        [Required]
        public HelpType HelpType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}