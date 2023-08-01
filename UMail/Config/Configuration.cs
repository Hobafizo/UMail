using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMail.Config.Entities;
using UMail.IO;
using UMail.Misc;

namespace UMail.Config
{
    internal static class Configuration
    {
        #region Constant Settings
        private const string    ConfigFile = "settings.ini",
                                EmailFile  = "emails.txt";
        #endregion

        #region Members
        private static Dictionary<string, EmailService> m_emails = new Dictionary<string, EmailService>();

        public static bool Loaded { get; private set; }
        #endregion

        #region Methods
        public static void Load()
        {
            try
            {
                // Creates config files on first use
                CreateFirstUse();

                // Load general config
                IniFile cfg = new IniFile(ConfigFile);

                // Load other config files
                LoadEmails();

                // Finalize
                Logger.Write(LogType.Config, ConsoleColor.Green, "Settings {0}loaded successfully.", Loaded ? "re" : "");
                Loaded = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred on loading configuration.");
            }
        }

        private static void CreateFirstUse()
        {
            try
            {
                // Create non-existing config files
                if (!File.Exists(ConfigFile))
                    File.CreateText(ConfigFile);
                if (!File.Exists(EmailFile))
                    File.CreateText(EmailFile);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred on preparing first use configuration.");
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
                            service = new EmailService()
                            {
                                Department = data[0],
                                ServerHost = data[1],
                                ServerPort = int.Parse(data[2]),
                                UseSSL = true,
                                Email = data[3],
                                Password = data[4]
                            };

                        else if (data.Length == 6) // Format => Department:Host:Port:Email:Password:UseSSL
                            service = new EmailService()
                            {
                                Department = data[0],
                                ServerHost = data[1],
                                ServerPort = int.Parse(data[2]),
                                UseSSL = bool.Parse(data[5]),
                                Email = data[3],
                                Password = data[4]
                            };

                        if (service != null)
                        {
                            if (!m_emails.ContainsKey(service.Department))
                                m_emails.Add(service.Department, service);
                            else
                                Logger.Warn(LogType.Config, "[{0}] department email service is duplicated, first match only will count.", service.Department);
                        }
                    }
                }

                if (m_emails.Count > 0)
                    Logger.Write(LogType.Config, ConsoleColor.DarkGreen, "{0} email services are set.", m_emails.Count);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred on loading email services.");
            }
        }
        #endregion

        #region Getters
        private static KeyValuePair<string, EmailService>[] Emails
        {
            get
            {
                return m_emails.ToArray();
            }
        }
        #endregion
    }
}
