using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UMail.Engines;
using UMail.Configuration;
using UMail.Configuration.Entities;
using UMail.Misc;

namespace UMail.Services
{
    public class EmailService
    {
        #region Members
        public string Department { get; private set; }
        public string ServerHost { get; private set; }
        public int ServerPort { get; private set; }
        public bool UseSSL { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public int SentCount { get; private set; }
        public int FailedCount { get; private set; }

        private SmtpClient m_client;
        private NetworkCredential m_credentials;
        #endregion

        #region Methods
        public EmailService(string department, string serverhost, int serverport, string email, string password, bool useSSL = false)
        {
            Department = department;
            ServerHost = serverhost;
            ServerPort = serverport;
            Email = email;
            Password = password;
            UseSSL = useSSL;

            SentCount = FailedCount = 0;

            // logon credentials
            m_credentials = new NetworkCredential();
            m_credentials.UserName = Email;
            m_credentials.Password = Password;

            m_client = new SmtpClient();

            // connection settings
            m_client.Host = ServerHost;
            m_client.Port = ServerPort;
            m_client.EnableSsl = UseSSL;

            // authenication info
            m_client.Credentials = m_credentials;
            
            // init event handlers
            m_client.SendCompleted += OnSendCompleted;
        }

        ~EmailService()
        {
        }

        public void SendEmail(string sender_name, string recipient_mail, string recipient_name, string subject, string body, bool html = false)
        {
            MailAddress from = new MailAddress(Email, sender_name);
            MailAddress recipient = new MailAddress(recipient_mail, recipient_name);

            MailMessage message = new MailMessage(from, recipient);
            message.Subject = subject;
            message.IsBodyHtml = html;

            if (html)
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html));
            else
                message.Body = body;

            m_client.SendAsync(message, Department);
        }
        #endregion

        #region Event Handlers
        private void OnSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string? department = (string?)e.UserState;

            if (!e.Cancelled && e.Error == null)
            {
                SentCount++;
                GlobalMgr.SetTitle(sent: 1);
            }
            else
            {
                FailedCount++;
                GlobalMgr.SetTitle(failed: 1);

                if (e.Error != null)
                    Logger.OnLog(LogProperties.Error, e.Error);
            }
        }
        #endregion
    }
}
