using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidget22;

namespace WindowsFormsApp1
{
    class InputInfo
    {
        private VoltageRatioInput input;
        private string name;
        public double value { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public VoltageRatioInput Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                if (null != input)
                {
                    name = input.DeviceClassName + "(" + input.DeviceSerialNumber.ToString() + ", Channel: " + input.Channel.ToString() + ")";
                }
                else
                {
                    name = "";
                }
            }
        }

        public InputInfo()
        {
            Input = null;
        }

        public InputInfo(VoltageRatioInput inp)
        {
            Input = inp;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
