using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class handleClient
    {
        private TcpClient _clientSocket = null;
        public IDictionary<TcpClient, string> _clientList = null;

        public void startClient(TcpClient clientSocket, Dictionary<TcpClient, string> clientList)
        {
            this._clientSocket = clientSocket;
            this._clientList = clientList;

            Thread t_hanlder = new Thread(doChat);
            t_hanlder.IsBackground = true;
            t_hanlder.Start();
        }

        public delegate void MessageDisplayHandler(string message, string user_name);

        public event MessageDisplayHandler OnReceived;


        public delegate void DisconnectedHandler(TcpClient clientSocket);

        public event DisconnectedHandler OnDisconnected;


        private void doChat()
        {
            NetworkStream stream = null;

            try
            {
                byte[] buffer = new byte[1024];
                string msg = string.Empty;
                int bytes = 0;
                int MessageCount = 0;

                while (true)
                {
                    MessageCount++;
                    stream = _clientSocket.GetStream();
                    bytes = stream.Read(buffer, 0, buffer.Length);
                    msg = Encoding.Unicode.GetString(buffer, 0, bytes);
                    msg = msg.Substring(0, msg.IndexOf("$"));

                    if (OnReceived != null)
                        OnReceived(msg, _clientList[_clientSocket].ToString());
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("doChat - SocketException : {0}", se.Message));

                if (_clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(_clientSocket);

                    _clientSocket.Close();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("doChat - Exception : {0}", ex.Message));

                if (_clientSocket != null)
                {
                    if (OnDisconnected != null)
                        OnDisconnected(_clientSocket);

                    _clientSocket.Close();
                    stream.Close();
                }
            }
        }
    }
}
