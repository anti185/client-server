using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace Client
{
    public class ClientManager
    {
        readonly Socket _socket;
        bool _connected;
        #region Delegate
        public delegate void ReceivedEventHandler(ClientManager cs, string received);
        public delegate void DisconnectedEventHandler(ClientManager cs);
        #endregion
        #region Event
        public event ReceivedEventHandler Received = delegate { };
        public event EventHandler Connected = delegate { };
        public event DisconnectedEventHandler Disconnected = delegate { };
        #endregion

        public ClientManager()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect(string Ip, int Port)
        {
            IPEndPoint EndIpPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);
            _socket.BeginConnect(EndIpPoint,new AsyncCallback(CallbackConnect), null);
        }
        private void CallbackConnect(IAsyncResult ar)
        {
            _socket.EndConnect(ar);
            _connected = true;
            if (Connected != null) Connected.Invoke(this,EventArgs.Empty);
            byte[] buffer = new byte[_socket.SendBufferSize];
            _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(CallbackReceive), buffer);
        }
        private void CallbackReceive(IAsyncResult ar)
        {
            var buffer = (byte[])ar.AsyncState;
            var rec = _socket.EndReceive(ar);
            if (rec != 0)
            {
                var data = Encoding.ASCII.GetString(buffer, 0, rec);
                if (Received != null) Received.Invoke(this, data);
            }
            else
            {
                if (Disconnected != null) Disconnected.Invoke(this);
                _connected= false;
                Close();
                return;
            }
            _socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(CallbackReceive), buffer);
        }
        public void Send(string str)
        {
            try
            {
                var buffer = Encoding.ASCII.GetBytes(str);
                _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, ar => _socket.EndSend(ar), null);
            }
            catch { Disconnected.Invoke(this); }
        }

        private void Close()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}
