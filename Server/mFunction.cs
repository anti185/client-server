using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
   static class mFunction
    {

        static List<Socket> clients = new List<Socket>();

        static public void Accept(Socket socket)
        {
            Client client = new Client(socket);
            client.Received += Received;
            client.Disconnected += discon;
            clients.Add(socket);
        }
        static void Received(Client client, byte[] data)
        {
            Console.WriteLine("Client : " + client.Ip + " Message: " + Encoding.ASCII.GetString(data));
            Broadcast("hey mean");
        }
        static void discon(Client client,Socket soc)
        {
            Console.WriteLine("Client : " + client.Ip + " Disconnected");
            clients.Remove(soc);

        }
        static void Broadcast(string str)
        {
            foreach(var handle in clients)
            {
                handle.Send(Encoding.ASCII.GetBytes(str));
            }
        }
    }
}
