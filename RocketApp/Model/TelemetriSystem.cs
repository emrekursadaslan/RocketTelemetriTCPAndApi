using System;
using System.Collections.Generic;
using System.Text;

namespace RocketApp
{
    public class TelemetriSystem
    {
        public header header { get; set; }
        public playloadTelemetri playload { get; set; }
        public footer footer { get; set; }
    }
    public class header
    {
        public byte startByte { get; set; }
        public string Id { get; set; }
        public byte packetNumber { get; set; }
        public byte packetSize{ get; set; }
    }
    public class playloadTelemetri
    {
        public float Altitude { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float Thrust { get; set; }
        public float Temperature { get; set; }
    }
    public class footer
    {
        public short CRC16 { get; set; }
        public byte Delimiter { get; set; }

    }
}
