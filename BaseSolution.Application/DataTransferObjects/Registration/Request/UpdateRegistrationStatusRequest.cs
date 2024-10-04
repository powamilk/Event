using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Registration.Request
{
    public class UpdateRegistrationStatusRequest
    {
        public string Status { get; set; }
        public int Id { get; set; }
    }
}
