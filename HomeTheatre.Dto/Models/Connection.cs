using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTheatre.Dto.Models
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public int RoomId { get; set; }
        public string UserName { get; set; }
        public bool Connected { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
