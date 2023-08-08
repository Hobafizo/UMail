using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UMail.Misc;
using UMail.Network;

namespace UMail.Engines
{
    public static class GlobalMgr
    {
        #region Members
        private static int m_sentemailcount = 0, m_failedemailcount = 0;
        
        public static int SentEmailCount
        {
            get
            {
                return m_sentemailcount;
            }
            set
            {
                Interlocked.Exchange(ref m_sentemailcount, value);
            }
        }
        public static int FailedEmailCount
        {
            get
            {
                return m_failedemailcount;
            }
            set
            {
                Interlocked.Exchange(ref m_failedemailcount, value);
            }
        }

        public static ServerListener MainServer { get; private set; }
        #endregion

        #region Methods
        public static void SetTitle(int sent = 0, int failed = 0)
        {
            if (sent > 0)
                SentEmailCount += sent;
            if (failed > 0)
                FailedEmailCount += failed;

            string? appname = Assembly.GetExecutingAssembly().GetName().Name;
            Console.Title = string.Format("{0} (sent: {1} - failed: {2})", appname ?? "UMailer", SentEmailCount, FailedEmailCount);
        }

        public static void StartServerListen(string host, int port)
        {
            if (MainServer == null)
            {
                MainServer = new ServerListener(host, port);
                if (MainServer.Start())
                    Logger.Write(LogType.Main, ConsoleColor.Cyan, "Server is listenning on {0}:{1}", host, port);
            }
        }
        #endregion
    }
}
