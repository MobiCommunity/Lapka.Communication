using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Communication.Api.Models.Request
{
    public class CreateMessageRequest
    {
        [Required]
        public string Description { get; set; }
    }
}