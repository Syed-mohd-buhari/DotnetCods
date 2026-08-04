
using System.Net;
using System.Net.Mail;

namespace CAM.Mail
{
    public class MailManager : IMailManager
    {
        private string _Host;
        private int _Port;
        private NetworkCredential _Credentials;
        private bool _EnableSsl;
        private SmtpClient _smtpClient;
        private string _encryptionMethod;

        public MailManager(string host, int port, string username, string password, bool enableSsl, string encryptionMethod)
        {
            _Port = port;
            _Credentials = new NetworkCredential(username, password);
            _EnableSsl = enableSsl;
            _Host = host;
            _encryptionMethod = encryptionMethod;
        }
        public MailManager(MailContextOptionsBuilder builder) : this(builder.Host, builder.Port, builder.Username, builder.Password, builder.EnableSsl, builder.EncryptionMethod)
        {
        }

        public void Connect()
        {
            _smtpClient = new SmtpClient(_Host)
            {
                Port = _Port,
                EnableSsl = _EnableSsl,
                UseDefaultCredentials = false,

            };
            switch (_encryptionMethod)
            {
                case "STARTTLS":
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    break;
                default:
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                    break;
            }
            _smtpClient.Credentials = _Credentials;

        }
        public void Diconnect()
        {
            if (_smtpClient != null)
            {
                _smtpClient.Dispose();
            }
            _smtpClient = null;
        }
        public void SendMail(MailMessage mailMessage)
        {
        //    bool justConnected = true;
        //    if (_smtpClient == null)
        //    {
        //        this.Connect();
        //        justConnected = false;
        //    }
        //    _smtpClient.Send(mailMessage);

        //    if (!justConnected)
        //    {
        //        this.Diconnect();
        //    }
        }
    }
}