using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Review.Request
{
    public class ReviewFromOrganizersRequest
    {
        public int EventId { get; set; }
        public int ParticipantId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
    }
}
