using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMail.Configuration;
using UMail.Configuration.Entities;

namespace UMail.Misc
{
    public enum LogType : byte
    { 
        General,
        Main,
        Config,
        Logger
    }

    internal static class Logger
    {
        #region Constants
        private const string WarningFile = "warnings.txt",
                             ErrorFile   = "errors.txt";
        #endregion

        #region Members
        private static readonly object LogLocker = new object(),
                                       FileLocker = new object();
        #endregion

        #region Methods
        public static void Write(string message, params object[] args)
        {
            Write(LogType.General, ConsoleColor.Gray, message, args);
        }

        public static void Write(ConsoleColor color, string message, params object[] args)
        {
            Write(LogType.General, color, message, args);
        }

        public static void Write(LogType type, string message, params object[] args)
        {
            Write(type, ConsoleColor.Gray, message, args);
        }

        public static void Write(LogType type, ConsoleColor color, string message, params object[] args)
        {
            lock (LogLocker)
            {
                if (color != ConsoleColor.Gray)
                    Console.ForegroundColor = color;

                Console.WriteLine(type != LogType.General ? string.Format("[{0}] {1}", type.ToString(), message) : message, args);
                Console.ResetColor();
            }
        }

        public static void Debug(string message, params object[] args)
        {
            Debug(LogType.General, ConsoleColor.Gray, message, args);
        }

        public static void Debug(ConsoleColor color, string message, params object[] args)
        {
            Debug(LogType.General, color, message, args);
        }

        public static void Debug(LogType type, string message, params object[] args)
        {
            Debug(type, ConsoleColor.Gray, message, args);
        }

        public static void Debug(LogType type, ConsoleColor color, string message, params object[] args)
        {
            Write(type, color, string.Format("[{0}] {1}", DateTime.Now.ToString(), message), args);
        }

        public static void Warn(string message, params object[] args)
        {
            Warn(LogType.General, message, args);
        }

        public static void Warn(LogType type, string message, params object[] args)
        {
            Write(type, ConsoleColor.DarkYellow, message, args);
        }

        public static void Error(Exception exception, string message, params object[] args)
        {
            Error(LogType.General, exception, message, args);
        }

        public static void Error(LogType type, Exception exception, string message, params object[] args)
        {
            Error(type, string.Format("{0}\n\nError details: {1}", message, exception.ToString()), args);
        }

        public static void Error(string message, params object[] args)
        {
            Error(LogType.General, message, args);
        }

        public static void Error(LogType type, string message, params object[] args)
        {
            Debug(type, ConsoleColor.DarkRed, message, args);
        }

        private static void Log(LogProperties level, string message, params object[] args)
        {
            lock (FileLocker)
            {
                try
                {
                    string? filename = GetLogFile(level);

                    if (filename != null)
                    {
                        using (StreamWriter writer = new StreamWriter(filename, true))
                        {
                            writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now, message), args);
                            writer.Close();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(LogType.Logger, exception, "Error occurred on file logging.");
                }
            }
        }
        #endregion

        #region Event Handlers
        public static void OnLog(LogProperties level, Exception exception)
        {
                OnLog(level, LogType.General, exception);
        }

        public static void OnLog(LogProperties level, LogType type, Exception exception)
        {
            if ((Config.Settings.LogLevel & level) == level)
                OnLog(level, type, string.Format("{0}\n", exception.ToString()), true);
        }

        public static void OnLog(LogProperties level, string message, bool pass = false, params object[] args)
        {
            OnLog(level, LogType.General, message, pass, args);
        }

        public static void OnLog(LogProperties level, LogType type, string message, bool pass = false, params object[] args)
        {
            if (pass || (Config.Settings.LogLevel & level) == level)
            {
                switch (level)
                {
                    case LogProperties.Warning:
                        {
                            Warn(type, message, args);
                            break;
                        }

                    case LogProperties.Error:
                        {
                            Error(type, message, args);
                            break;
                        }

                    default:
                        return;
                }

                if ((Config.Settings.LogLevel & LogProperties.FileLog) == LogProperties.FileLog)
                    Log(level, message, args);
            }
        }
        #endregion

        #region Getters
        private static string? GetLogFile(LogProperties level)
        {
            switch (level)
            {
                case LogProperties.Warning:
                    return WarningFile;

                case LogProperties.Error:
                    return ErrorFile;
            }
            return null;
        }
        #endregion
    }
}
