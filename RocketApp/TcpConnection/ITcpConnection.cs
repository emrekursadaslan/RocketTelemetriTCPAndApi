using System;
using System.Collections.Generic;
using System.Text;

namespace RocketApp
{
    public interface ITcpConnection
    {
        bool OpenConnection();

        bool CloseConnection();

        int SendData(byte[] data);

        int Receive(out byte[] data);
    }
}
