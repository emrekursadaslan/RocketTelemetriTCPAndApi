using System.Collections.Generic;
using System.Threading;

namespace RocketApp
{
    public enum ClientServer
    {
        client,
        server
    }
    public class TcpManager
    {
        public readonly ITcpConnection _tcpConnection;
        private Queue<byte> _readBytes;
        private Thread _readWorker;
        private object _readLock;
        public TcpManager(string ip, int port, ClientServer clientserver)
        {
            _readBytes = new Queue<byte>();
            _readLock = new object();
            if (clientserver == ClientServer.client)
            {
                _tcpConnection = new TcpClient(ip, port);
            }
            else
            {
                _tcpConnection = new TcpServer(ip, port);
            }
        }
        public int Write( byte[] data)
        {
            return _tcpConnection.SendData(data);
        }
        public int Read(out byte[] data, int size)
        {
            if(_readBytes.Count < size)
            {
                data = null;
                return -1;
            }
            data = new byte[size];
            lock (_readLock)
            {
                for (int i = 0; i < size; i++)
                {
                    data[i] = _readBytes.Dequeue();
                }
            }
            return size;
        }
        public void ReadWorker()
        {
            while (true)

            {
                byte[] data;
                int size =_tcpConnection.Receive(out data);
                lock (_readLock)
                {
                    for (int i = 0; i < size; i++)
                    {
                        _readBytes.Enqueue(data[i]);
                    }
                }
                Thread.Sleep(10);
            }
        }

        public bool OpenConnection()
        {
            if (_tcpConnection.OpenConnection() && _tcpConnection.GetType().Equals(new TcpClient().GetType()))
            {
                _readWorker = new Thread(ReadWorker);
                _readWorker.Start();
                return true;
            }
            return false;
        }
        public bool CloseConnection()
        {
            _tcpConnection.CloseConnection();
            _readWorker.Abort();
            return true;
        }
    }
}
