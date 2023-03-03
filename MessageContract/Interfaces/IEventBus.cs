using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContract.Interfaces
{
    public interface IEventBus
    {
        public DateTime PublishedTime { get; set; }
        public Guid QueueId { get; set; }
        public string OrganizationCode { get; set; }
    }
}
