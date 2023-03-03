using MessageContract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContract.Events
{
    public interface IOrderEvent:IEventBus
    {
        Guid OrderId { get; set; }
        int StatusCode { get; set; }
        int OrderType { get; set; }
    }
}
