using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMail.Configuration.Entities
{
    public enum LogProperties : byte
    {
        None = 0,
        Warning = 1,
        Error = 2,
        FileLog = 4
    }
}
