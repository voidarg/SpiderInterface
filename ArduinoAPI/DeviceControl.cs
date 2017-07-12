using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoAPI
{
    interface DeviceControl
    {
        
        bool IsAttached { get; }
        void SendCommand ();
    }
}
