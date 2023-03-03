using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContract.Models
{
    public class ConfigurationOption
    {
        public string OrganizationCode { get; set; }

        public string Environment { get; set; }

        public Type CacheServiceType { get; set; }

        public Type CacheResultModelType { get; set; }

        public string CacheResultProperty { get; set; }

        public string CacheGetMethod { get; set; } = "Get";


        public string CacheSetMethod { get; set; } = "Set";


        public string ConfigApiUrl { get; set; }
    }
}
