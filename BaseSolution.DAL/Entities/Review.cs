using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int ParticipantId { get; set; }
        public float Rating { get; set; } 
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Event Event { get; set; }
        public Participant Participant { get; set; }
    }
}
