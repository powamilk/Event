using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Registration
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventLocation { get; set; }
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
    }
}
