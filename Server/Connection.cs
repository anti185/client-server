using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Connection
    {
        private Socket _socket;
        public int Port{ get; private set;}
        public IPAddress Ip { get; private set; }
        public bool Listen { get; private set; }

        #region delegate
        public delegate void SocketAcceptedHandler(Socket e);
        #endregion

        #region event
        public event SocketAcceptedHandler SocketAccepted;
        #endregion

        public Connection(int port, IPAddress ip)
        {
            this.Port = port;
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Ip = ip;
        }
        public void Start()
        {
            if (Listen) return;
            _socket.Bind(new IPEndPoint(Ip,Port));
            _socket.Listen(10);
            _socket.BeginAccept(new AsyncCallback(CallbackAccept),null);
            Listen = true;
        }
        public void Stop()
        {
            if (!Listen) return;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void CallbackAccept(IAsyncResult ar)
        {
            try
            {
                Socket handler = _socket.EndAccept(ar);
                if (SocketAccepted != null) SocketAccepted.Invoke(handler);
                _socket.BeginAccept(new AsyncCallback(CallbackAccept), null);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }
}
