using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            Connection con = new Connection(11000, IPAddress.Any);
            con.SocketAccepted += mFunction.Accept;
            con.Start();
            Console.ReadLine();
        }

       
    }
}
