using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Client
    {
        byte[] buffer;
        private Socket _socket;
        public string Ip { get; private set; }

        #region delegate
        public delegate void ClientReceivedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender, Socket handler);
        #endregion
        #region event
        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;
        #endregion

        public Client(Socket handler)
        {
            this.Ip = Convert.ToString(((IPEndPoint)handler.RemoteEndPoint).Address);
            this._socket = handler;
            _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, ReceiveCallback, null);
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndReceive(ar);
                var buffer = new byte[_socket.ReceiveBufferSize];
                var rec = _socket.Receive(buffer, buffer.Length, 0);
                if (rec < buffer.Length)
                {
                    Array.Resize(ref buffer, rec);
                }
                Received?.Invoke(this, buffer);
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, ReceiveCallback, null);

            }
            catch (Exception)
            {
                Close();
                Disconnected?.Invoke(this,_socket);
            }
        }
        public void Send(string data)
        {
            var buffer = Encoding.ASCII.GetBytes(data);
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, ar => _socket.EndSend(ar), null);
        }

        public void Close()
        {
            _socket.Dispose();
            _socket.Close();
        }
    }
}
