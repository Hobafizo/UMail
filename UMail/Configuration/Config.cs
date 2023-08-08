using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMail.Configuration.Entities;
using UMail.IO;
using UMail.Misc;
using UMail.Services;
using Newtonsoft.Json;

namespace UMail.Configuration
{
    internal static class Config
    {
        #region Constants
        private const string    ConfigFile = "config/settings.json",
                                EmailFile  = "config/emails.txt";
        #endregion

        #region Members
        private static Settings m_settings;
        private static Dictionary<string, EmailService> m_emails = new Dictionary<string, EmailService>();

        public static bool Loaded { get; private set; }
        public static Settings Settings { get { return m_settings; } }
        private static KeyValuePair<string, EmailService>[] Emails { get { return m_emails.ToArray(); } }
        private static int EmailCount => m_emails.Count;
        #endregion

        #region Methods
        public static void Load()
        {
            try
            {
                // Creates config files on first use
                CreateFirstUse();

                // Load general config
                m_settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(ConfigFile));

                // Load other config files
                LoadEmails();

                // Finalize
                Logger.Write(LogType.Config, ConsoleColor.Green, "Settings {0}loaded successfully.", Loaded ? "re" : "");
                Loaded = true;
            }
            catch (Exception ex)
            {
                Logger.Error(LogType.Config, ex, "Error occurred on loading.");
            }
        }

        private static void CreateFirstUse()
        {
            try
            {
                // Create non-existing config files
                if (!File.Exists(ConfigFile))
                {
                    using (StreamWriter writer = new StreamWriter(ConfigFile, false))
                    {
                        writer.Write(JsonConvert.SerializeObject(m_settings, Formatting.Indented));
                        writer.Close();
                    }
                }

                if (!File.Exists(EmailFile))
                    File.CreateText(EmailFile).Close();
            }
            catch (Exception ex)
            {
                Logger.Error(LogType.Config, ex, "Error occurred on preparing first use config.");
            }
        }

        private static void LoadEmails()
        {
            try
            {
                // Clear email services
                m_emails.Clear();

                // Reload email services
                string[] data;
                EmailService? service;

                foreach (string line in File.ReadAllLines(EmailFile))
                {
                    if (!line.TrimStart().StartsWith("//")) //custom config file comment
                    {
                        service = null;
                        data = line.Split(':');

                        if (data.Length == 5) // Format => Department:Host:Port:Email:Password
                            service = new EmailService(data[0], data[1], int.Parse(data[2]), data[3], data[4]);

                        else if (data.Length == 6) // Format => Department:Host:Port:Email:Password:UseSSL
                            service = new EmailService(data[0], data[1], int.Parse(data[2]), data[3], data[4], bool.Parse(data[5]));

                        if (service != null)
                        {
                            if (!m_emails.ContainsKey(service.Department))
                                m_emails.Add(service.Department, service);
                            else
                                Logger.OnLog(LogProperties.Warning, LogType.Config, "[{0}] department email service is duplicated, first match only will count.", false, service.Department);
                        }
                    }
                }

                if (m_emails.Count > 0)
                    Logger.Write(LogType.Config, ConsoleColor.DarkGreen, "{0} email services are set.", m_emails.Count);
            }
            catch (Exception ex)
            {
                Logger.Error(LogType.Config, ex, "Error occurred on loading email services.");
            }
        }
        #endregion

        #region Getters
        public static EmailService? GetEmailService(string department)
        {
            return m_emails.TryGetValue(department, out EmailService? service) ? service : null;
        }
        #endregion
    }
}
