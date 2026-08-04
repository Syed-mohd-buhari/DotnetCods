using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Mail
{
    public enum MailStateEnum : byte
    {
        Insert,
        Sending,
        Sended,
        ErrorOnSend
    }
}
