using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    class ArduinoInfo
    {
        public String Name { get; set; }
        public String Port { get; set; }

        public ArduinoInfo()
        {
            Name = "";
            Port = "";
        }

        public ArduinoInfo (string name, string port)
        {
            Name = name;
            Port = port;
        }

        public override string ToString()
        {
            return Name + " @ " + Port; 
        }

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
