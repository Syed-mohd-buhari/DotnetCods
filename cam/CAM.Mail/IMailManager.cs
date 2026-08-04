using System.Net.Mail;

namespace CAM.Mail
{
    public interface IMailManager
    {
        public void SendMail(MailMessage mailMessage);
        public void Connect();
        public void Diconnect();
    }
}
