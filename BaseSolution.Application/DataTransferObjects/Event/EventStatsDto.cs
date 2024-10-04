using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Event
{
    public class EventStatsDto
    {
        public int TotalEvents { get; set; }
        public int TotalParticipants { get; set; }
        public double AverageParticipantsPerEvent { get; set; }
        public List<EventStatusStats> EventsByStatus { get; set; }
        public EventsInDateRangeDto EventsInDateRange { get; set; }
    }

}
