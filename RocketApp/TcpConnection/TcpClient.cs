using System;
using System.Net;
using System.Net.Sockets;

namespace RocketApp
{
    public class TcpClient : ITcpConnection
    {
        private Socket _socket { get; }
        private IPEndPoint _endPoint;
        private byte[] _tempData;

        public TcpClient( string endPointIp, int endPort)
        {
            try
            {
                if (IPAddress.TryParse(endPointIp, out IPAddress ipAddress))
                {
                    _endPoint = new IPEndPoint(ipAddress, endPort);
                    _socket = new Socket(ipAddress.AddressFamily,
                                        SocketType.Stream, ProtocolType.Tcp);

                    _tempData = new byte[1024];
                }
            }
            catch (Exception e)
            {
            }
        }

        public TcpClient()
        {
        }

        public bool OpenConnection()
        {
            if (_socket == null)
                return false;

            try
            {
                _socket.Connect(_endPoint);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool CloseConnection()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int SendData(byte[] data)
        {
            _socket.Send(data);

            byte[] readBytes;
            Receive(out readBytes);

            return data.Length;
        }

        public int Receive(out byte[] data)
        {
            int byteRecv = _socket.Receive(_tempData);
            data = _tempData;

            return byteRecv;
        }
    }
}
