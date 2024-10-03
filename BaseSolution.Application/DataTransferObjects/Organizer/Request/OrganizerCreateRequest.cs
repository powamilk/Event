using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Organizer.Request
{
    public class OrganizerCreateRequest
    {
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
    }
}
