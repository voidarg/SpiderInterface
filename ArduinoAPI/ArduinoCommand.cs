using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoAPI
{
    class ArduinoCommand
    {
        public enum CommandID
        {
            Noop,
            Move,
            SetLimits
        };

        byte commandId;

  }
}