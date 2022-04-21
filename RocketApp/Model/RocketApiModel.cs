using System;
using System.Collections.Generic;
using System.Text;

namespace RocketApp
{
    public class RocketApiModel
    {
        public string id { get; set; }
        public string model { get; set; }
        public uint mass { get; set; }
        public playload playload { get; set; }
        public telemetry telemetry { get; set; }
        public string status { get; set; }
        public timestamps timestamps { get; set; }
        public double altitude { get; set; }
        public double speed { get; set; }
        public double acceleration { get; set; }
        public ulong thrust { get; set; }
        public float temperature { get; set; }


    }
    public class playload
    {
        public string description { get; set; }
        public ushort weight { get; set; }
    }
    public class telemetry
    {
        public string host { get; set; }
        public ushort port { get; set; }
    }    
    public class timestamps
    {
        public string launched { get; set; }
        public string deployed { get; set; }
        public string failed { get; set; }
        public string cancelled { get; set; }
    }

}
