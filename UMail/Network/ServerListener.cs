using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UMail.Misc;
using UMail.Configuration.Entities;

namespace UMail.Network
{
    public class ServerListener
    {
        #region Members
        private bool m_listen;
        private string m_host;
        private int m_port;
        private IPEndPoint m_endpoint;
        private TcpListener m_listener;

        public bool Listenning { get { return m_listen; } }
        public string Host { get{return m_host; } }
        public int Port { get { return m_port; } }
        #endregion

        #region Methods
        public ServerListener(string host, int port)
        {
            m_listen = false;
            m_host = host;
            m_port = port;

            m_endpoint = new IPEndPoint(IPAddress.Parse(m_host), m_port);
            m_listener = new TcpListener(m_endpoint);
        }

        ~ServerListener()
        {
            Stop();
        }

        public bool Start(int acceptsToPost = 1)
        {
            try
            {
                if (!m_listen && acceptsToPost > 0)
                {
                    m_listen = true;
                    m_listener.Start();

                    for (int i = 0; i < acceptsToPost; ++i)
                        Accept();
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logger.OnLog(LogProperties.Error, exception);
            }
            return false;
        }

        public bool Stop()
        {
            try
            {
                if (m_listen)
                {
                    m_listen = false;
                    m_listener.Stop();
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logger.OnLog(LogProperties.Error, exception);
            }
            return false;
        }

        private async void Accept()
        {
            try
            {
                if (m_listen)
                {
                    Socket socket = await m_listener.AcceptSocketAsync();

                    ServerContext client = new ServerContext(this, socket);
                    client.BeginReceive();

                    Accept();
                }
            }
            catch (Exception exception)
            {
                Logger.OnLog(LogProperties.Error, exception);
            }
        }
        #endregion
    }
}
