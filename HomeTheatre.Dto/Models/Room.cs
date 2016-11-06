using System;
using System.ComponentModel.DataAnnotations;

namespace HomeTheatre.Dto.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public string VideoSource { get; set; }
    }
}