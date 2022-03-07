using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPIP_Service
{
    public class Server
    {
        private Socket socket;
        
        public Server()
        {
            IPAddress local = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(local, 13000);
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iep);
            socket.Listen(20);
            
        }

        public void Open()
        {
            byte[] buffer = new byte[256];
            socket.BeginAccept(AsyncAccept, socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, AsyncReceive, buffer);
        }

        private void AsyncAccept(IAsyncResult asyncResult)
        {
            Socket socket = (Socket)asyncResult.AsyncState;
            socket.EndAccept(asyncResult);
        }

        private void AsyncReceive(IAsyncResult asyncResult)
        {

        }

        public void Write(string Data)
        {
            var b_data = Encoding.ASCII.GetBytes(Data);
            socket.Send(b_data);
        }


    }
}
