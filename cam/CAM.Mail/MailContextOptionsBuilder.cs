using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Mail
{
    public class MailContextOptionsBuilder
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string EncryptionMethod { get; set; }
    }
}
