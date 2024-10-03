using BaseSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Event.Request
{
    public class ViewEventWithPaginationReques
    {
        public string? Name { get; set; }
        public EntityStatus? Status { get; set; }
    }
}
