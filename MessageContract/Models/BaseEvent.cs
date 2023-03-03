using MessageContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContract.Models
{
    public class BaseEvent : IEventBus
    {
        public DateTime PublishedTime { get; set; } = DateTime.UtcNow;
        public Guid QueueId { get; set; } = Guid.NewGuid();
        public string OrganizationCode { get; set; } = "US";
    }
}
