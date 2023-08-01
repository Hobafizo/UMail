using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMail.Misc
{
    public enum LogType : byte
    { 
        General,
        Main,
        Config
    }

    internal static class Logger
    {
        #region Members
        private static readonly object LogLocker = new object();
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

        public static void Error(string message, params object[] args)
        {
            Error(LogType.General, message, args);
        }

        public static void Error(Exception exception, string message, params object[] args)
        {
            Error(LogType.General, string.Format("{0}\n\nError details: {1}", message, exception.ToString()), args);
        }

        public static void Error(LogType type, string message, params object[] args)
        {
            Debug(type, ConsoleColor.DarkRed, message, args);
        }
        #endregion
    }
}
