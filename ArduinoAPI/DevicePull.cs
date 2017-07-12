using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ArduinoAPI
{
    class RobotControl : IDisposable
    {
        private Arduino arms;
        private Arduino legs;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void Initialize()
        {
            var portNames = SerialPort.GetPortNames();

            // scan all ports opening each port and query device data
            foreach (var name in portNames)
            {
                try
                {
                    var port = new SerialPort(name, 115000); // try to initialize and open port
                    
                    // now when the port is successfully opened, try sending device identification requests

                    
                }
                catch (System.IO.IOException e)
                {

                }
                
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RobotControl() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
