using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    public class MotorInfo
    {
        public int Id { get;  }
        public int FidgetSensor { get; set; }
        public int Position { get;  set; }
        public int MinPosition { get; set; }
        public int MaxPosition { get; set; }
        public double Pressure { get; set; }
    }
}
