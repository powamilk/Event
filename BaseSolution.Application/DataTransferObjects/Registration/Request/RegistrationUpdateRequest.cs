using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Registration.Request
{
    public class RegistrationUpdateRequest
    {
        public int Id { get; set; }
        public int EventId { get; set; }  
        public int ParticipantId { get; set; }  
        public string Status { get; set; }

    }
}
