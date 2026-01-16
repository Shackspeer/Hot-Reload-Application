using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;


namespace FileService
{
    internal class Socket : IDisposable
    {
        private static List<IWebSocketConnection> _allConnection = new List<IWebSocketConnection>();
        private static WebSocketServer _server;


        public Socket(string port)
        {
            string location = $"ws://0.0.0.0:{port}";
            _server = new WebSocketServer(location);
        }


        public void StartServer()
        {
            _server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("State : Client has connected");
                    _allConnection.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("State : Client has disconnected");
                    _allConnection.Remove(socket);
                };
            });
        }


        public void SendReloadSignal()
        {
            foreach(IWebSocketConnection socket in _allConnection)
            {
                socket.Send("reload");
            }
        }



        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }

        }


    }
}
