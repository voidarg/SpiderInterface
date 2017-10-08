using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace WindowsFormsApp1
{
    class UdpReceiver
    {
        UdpClient receiver;
        private bool abort = false;
        public UdpReceiver(ref UdpClient _udpClient)
        {
            receiver = _udpClient;
        }
        public void Stop()
        {
            abort = true;
            receiver.Close();
        }
        public void ThreadRun()
        {
            abort = false;
            
            IPEndPoint ip = null;
            try
            {
                System.Diagnostics.Trace.WriteLine("Ready");
                while (!abort)
                {
                    byte[] data = receiver.Receive(ref ip);
                    string message = Encoding.Unicode.GetString(data);
                    //string message = Encoding.UTF8.GetString(data);                    
                    Console.WriteLine("UDPMessage: {0}", message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                System.Diagnostics.Trace.WriteLine("UdpReceiver Close");
                receiver.Close();
            }
        }
    }
}
