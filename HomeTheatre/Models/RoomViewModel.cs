using System.ComponentModel.DataAnnotations;

namespace HomeTheatre.Models
{
    public class RoomViewModel
    {
        public string Name { get; set; }
        [Display(Name = "Is this room private?")]
        public bool IsPrivate { get; set; }
        [Display(Name = "Video Source")]
        public string Source { get; set; }
    }

    public class UserViewModel
    {
        
    }
}