using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoAPI
{
    public class DeviceCommand
    {
        private byte[] buffer_;
        private int bufferSize_;

        /// <summary>
        /// Protected construction to be called by the derived class with max command buffer size specified
        /// </summary>
        /// <param name="bufferSize"></param>
        protected DeviceCommand(int bufferSize)
        {
            bufferSize_ = bufferSize;
            buffer_ = new byte[bufferSize];
        }

        protected delegate void GetBufferCallback(int maxSize, out byte[] buffer);
        
        
    }
}
