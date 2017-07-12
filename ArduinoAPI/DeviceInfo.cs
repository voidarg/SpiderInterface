using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoAPI
{
    public enum DeviceType
    {
        Arduino,
        FidgetBoard
    }

    interface IDeviceInfo
    {
        string Id { get; }
        DeviceType Type { get; }

    }
}
