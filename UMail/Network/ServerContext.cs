using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UMail.Misc;

namespace UMail.Network
{
    internal class ServerContext
    {
        #region Members
        private ServerListener m_listener;
        private Socket m_socket; 
        #endregion

        #region Methods
        public ServerContext(ServerListener listener, Socket socket)
        {
            m_listener = listener;
            m_socket = socket;

            Logger.Write(ConsoleColor.White, "New socket is open!");
        }

        public void BeginReceive()
        {

        }
        #endregion
    }
}
