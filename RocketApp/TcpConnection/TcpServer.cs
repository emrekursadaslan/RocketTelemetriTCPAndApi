using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RocketApp
{
    public class TcpServer : ITcpConnection
    {
        private IPEndPoint _endPoint;
        private Socket _socket;

        private List<Socket> _clients;

        private bool _isOpen;
        private Thread _acceptWorker;

        public TcpServer(string localIp, int port)
        {
            _clients = new List<Socket>();
            _isOpen = true;

            try
            {
                IPAddress ipAddress;
                if (IPAddress.TryParse(localIp, out ipAddress))
                {
                    _endPoint = new IPEndPoint(ipAddress, port);
                    _socket = new Socket(_endPoint.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                }
            }
            catch (Exception e)
            {

            }

        }

        public void AcceptWorker()
        {
            while (_isOpen)
            {

                Socket clientSocket = _socket.Accept();
                _clients.Add(clientSocket);
            }
        }

        public bool OpenConnection()
        {
            _socket.Bind(_endPoint);
            _socket.Listen(10);

            _acceptWorker = new Thread(AcceptWorker);
            _acceptWorker.Start();

            return true;
        }

        public bool CloseConnection()
        {
            _isOpen = false;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

            _acceptWorker.Abort();

            return true;
        }

        public int SendData(byte[] data)
        {
            foreach (var client in _clients)
            {
                client.Send(data);
            }

            return data.Length;
        }

        public int Receive(out byte[] data)
        {
            throw new NotImplementedException(); //Server data almayacak implemente edilmedi.
        }
    }
}
