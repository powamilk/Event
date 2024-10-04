using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Registration.Request
{
    public class BulkRegistrationRequest
    {
        public int EventId { get; set; }
        public List<int> ParticipantIds { get; set; }
    }
}
