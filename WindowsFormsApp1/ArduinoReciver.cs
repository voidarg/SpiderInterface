using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public class ArduinoReciver
    {
        Dictionary<int, double> AxisPosition = new Dictionary<int, double>();
        Dictionary<int, double> AxisPositionSum = new Dictionary<int, double>();
        Dictionary<int, Queue<double>> AxisPositionQueue = new Dictionary<int, Queue<double>>();
        Dictionary<int, int> MtrToPh;
        List<SerialPort> m_listofPorts;
        public ArduinoReciver(ref List<SerialPort> _listofPorts, ref Dictionary<int, int> _MtrToPh)
        {
            m_listofPorts = _listofPorts;
            MtrToPh = _MtrToPh;
        }

        public bool abort { get; set; }

        public int getValueOf(int index)
        {
            return (int)AxisPosition[index];
        }
        public int Count()
        {
            return AxisPosition.Count;
        }
        public void Start()
        {
            for (int i = 0; i < m_listofPorts.Count; i++)
            {
                m_listofPorts[i].DataReceived += SerialPort_DataReceived;
                m_listofPorts[i].ErrorReceived += SerialPort_ErrorReceived;
            }
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
                AxisPosition.Add(i, 1);
                AxisPositionSum.Add(i, 0);
                Queue<double> PositionPipe = new Queue<double>();
                AxisPositionQueue.Add(i, PositionPipe);
            }
            return true;
        }
        public void ThreadRun()
        {
            initializeArduinoDevices();
            System.Diagnostics.Trace.WriteLine("ArduinoReciver started");
            Thread.Sleep(2000);
            while (!abort)
            {
                Thread.Sleep(1000);
            }
            System.Diagnostics.Trace.WriteLine("ArduinoReciver stopped");
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            int i = 0;
            while (m_listofPorts[i++].PortName != sp.PortName) ;
            int motorShift = (i - 1) * 6;
            if (sp.IsOpen)
            {
                var data = sp.ReadExisting();
                MatchCollection matchMtr = Regex.Matches(data, @"{M(\d+),P(\d+)}");
                foreach (Match m in matchMtr)
                {
                    string[] tmp = m.ToString().Split(new string[] { ",P" }, StringSplitOptions.None);
                    //System.Diagnostics.Trace.WriteLine(int.Parse(tmp[0].Remove(0, 2)) + " " + int.Parse(tmp[1].TrimEnd('}')));
                    //System.Diagnostics.Trace.WriteLine(int.Parse(tmp[0].Remove(0, 2)).ToString());
                    int motorTrimmed = int.Parse(tmp[0].Remove(0, 2));
                    int posTrimmed = int.Parse(tmp[1].TrimEnd('}'));
                    int phidgetId = MtrToPh[motorTrimmed + motorShift];
                    //if (phidgetId == 11)
                    //{
                    //    System.Diagnostics.Trace.WriteLine(/*m.ToString() + " " + " " + */AxisPositionSum[phidgetId] + "," + posTrimmed + "," + AxisPosition[phidgetId]);
                    //    // System.Diagnostics.Trace.WriteLine(m.ToString() + " " + sp.PortName + " " + (motorTrimmed + motorShift) + " " + motorTrimmed + " " + posTrimmed);
                    //}

                    if (AxisPositionQueue[phidgetId].Count < 10)
                    {
                        AxisPositionQueue[phidgetId].Enqueue(posTrimmed);
                        AxisPositionSum[phidgetId] += posTrimmed;
                    }
                    else
                    {
                        if (Math.Abs(posTrimmed * 10 - AxisPositionSum[phidgetId]) > 30)
                        {
                            AxisPositionQueue[phidgetId].Enqueue(AxisPositionSum[phidgetId] / 10);
                            AxisPositionSum[phidgetId] += (AxisPositionSum[phidgetId] / 10);
                        }
                        else
                        {
                            AxisPositionQueue[phidgetId].Enqueue(posTrimmed);
                            AxisPositionSum[phidgetId] += posTrimmed;
                        }
                        AxisPositionSum[phidgetId] -= AxisPositionQueue[phidgetId].Dequeue();
                    }
                    AxisPosition[phidgetId] = AxisPositionSum[phidgetId] / 10;
                    //System.Diagnostics.Trace.WriteLine( m.ToString() + " " + sp.PortName + " " + (motorTrimmed + motorShift) + " " + motorTrimmed+ " " + posTrimmed);
                }
            }
        }

    }
}
