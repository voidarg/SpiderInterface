using System;
using System.Collections.Generic;
using System.Text;
using System.Management; // need to add System.Management to your project references.

namespace WindowsFormsApp1
{
    class OS
    {
        public static List<ArduinoInfo> GetUSBDevices()
        {
            List<ArduinoInfo> devices = new List<ArduinoInfo>();
            return devices;
        }

        public static List<ArduinoInfo> GetSerialDevices()
        {
            return AutodetectArduinoPortDevices();
        }

        private static List<ArduinoInfo> AutodetectArduinoPortDevices()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
            List<ArduinoInfo> devices = new List<ArduinoInfo>();

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Arduino"))
                    {
                        devices.Add(new ArduinoInfo (desc, deviceId));
                    }
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return devices;
        }

    }
}
