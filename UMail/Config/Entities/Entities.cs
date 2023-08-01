using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMail.Config.Entities
{
    public class EmailService
    {
        public string Department { get; set; }
        public string ServerHost { get; set; }
        public int ServerPort { get; set; }
        public bool UseSSL { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
