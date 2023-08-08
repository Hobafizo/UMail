using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMail.Configuration.Entities
{
    public struct Settings
    {
        public string ListenIP { get; set; }
        public int ListenPort { get; set; }
        public LogProperties LogLevel { get; set; }
        public int RequestBatchInterval { get; set; }
    }
}
