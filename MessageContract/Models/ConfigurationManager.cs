using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContract.Models
{
    public class ConfigurationManager
    {
        public string DataSource { get; set; }

        public string Domain { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }
        public string OrderEventApi { get; set; }
    }
}
