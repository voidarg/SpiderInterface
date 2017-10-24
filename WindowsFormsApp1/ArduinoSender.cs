using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApp1
{
    public class ArduinoSender
    {
        Dictionary<int, TorqueAndDir> AxisTorque = new Dictionary<int, TorqueAndDir>();
        Dictionary<int, char> AxisDir = new Dictionary<int, char>();
        Dictionary<int, int> PhToMtr;
        List<SerialPort> m_listofPorts;
        public ArduinoSender(ref List<SerialPort> _listofPorts, ref Dictionary<int, int> _PhToMtr)
        {
            m_listofPorts = _listofPorts;
            PhToMtr = _PhToMtr;
        }

        public bool abort { get; set; }

        public void setValueOf(int index, int val, char dir)
        {
            AxisTorque[index].Torque = val;
            AxisTorque[index].Direction = dir;
        }
        public int Count()
        {
            return AxisTorque.Count;
        }
        public void Start()
        {
        }
        public void Stop()
        {
            for (int i = 0; i < m_listofPorts.Count; i++)
            {
                //listofPorts[i].Close();
            }
        }
        bool initializeArduinoDevices()
        {
            for (int i = 0; i < 12; i++)
            {
                TorqueAndDir tmp = new TorqueAndDir();
                tmp.Torque = 0;
                tmp.Direction = 'F';
                AxisTorque.Add(i, tmp);
            }
            return true;
        }
        public void ThreadRun()
        {
            initializeArduinoDevices();
            System.Diagnostics.Trace.WriteLine("ArduinoSender started");
            Thread.Sleep(2000);
            while (!abort)
            {
                Thread.Sleep(1);
                //System.Diagnostics.Trace.WriteLine("ArduinoSender is ok:"+ AxisTorque.Count.ToString());
                for (int i = 0; i < AxisTorque.Count; i++)
                {
                    int SerialSelect = PhToMtr[i] / 6;
                    //string tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    string tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + ",0}");
                    //if (i == 10)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 11)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    if (i == 4)
                    {
                        tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    }
                    if (i == 5)
                    {
                        tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    }
                    //if (i == 3)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 8)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 9)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 2)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 0)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    //if (i == 1)
                    //{
                    //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                    //}
                    m_listofPorts[SerialSelect].Write(tmp);
                }
            }
            System.Diagnostics.Trace.WriteLine("ArduinoSender stopped");
        }
    }
}
