using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Communication.Application.Dto
{
    public class ValueDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

    }
}