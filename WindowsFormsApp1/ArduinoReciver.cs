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
        List<SerialPort> listofPorts = new List<SerialPort>();

        public ArduinoReciver(ref List<SerialPort> _listofPorts, ref Dictionary<int, int> _MtrToPh)
        {
            initializeArduinoDevices();
            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 115200;
            serialPort.PortName = "COM19";
            serialPort.WriteTimeout = 1000;
            serialPort.DtrEnable = false;
            serialPort.RtsEnable = false;
            serialPort.Open();
            listofPorts.Add(serialPort);
            listofPorts.Add(_listofPorts[1]);
            MtrToPh = _MtrToPh;
            for (int i = 0; i < listofPorts.Count; i++)
            {
                listofPorts[i].DataReceived += SerialPort_DataReceived;
                listofPorts[i].ErrorReceived += SerialPort_ErrorReceived;
            }
        }

        public bool abort { get; set; }

        public int getValueOf(int index)
        {
            if (AxisPosition.ContainsKey(index)) return (int)AxisPosition[index];
            return 1;
        }
        public int Count()
        {
            return AxisPosition.Count;
        }
        public void Start()
        {

        }
        public void Stop()
        {
            for (int i = 0; i < listofPorts.Count; i++)
            {
                //listofPorts[i].Close();
            }
        }
        bool initializeArduinoDevices()
        {
            for (int i = 0; i < 12; i++)
            {
                //AxisPosition.Add(i, 1);
                AxisPositionSum.Add(i, 0);
                Queue<double> PositionPipe = new Queue<double>();
                AxisPositionQueue.Add(i, PositionPipe);
            }
            return true;
        }
        public void ThreadRun()
        {
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
            //if (listofPorts[2].PortName == sp.PortName) return;
            while (listofPorts[i++].PortName != sp.PortName) ;
            int motorShift = (i-1)* 6;
            if (sp.IsOpen)
            {
                var data = sp.ReadExisting();
                MatchCollection matchMtr = Regex.Matches(data, @"{M(\d+),P(\d+)}");
                foreach (Match m in matchMtr)
                {
                    string[] tmp = m.ToString().Split(new string[] { ",P" }, StringSplitOptions.None);

                    int motorTrimmed = int.Parse(tmp[0].Remove(0, 2));
                    int posTrimmed = int.Parse(tmp[1].TrimEnd('}'));
                    int phidgetId = MtrToPh[motorTrimmed + motorShift];


                    if (AxisPositionQueue[phidgetId].Count < 10)
                    {
                        AxisPositionQueue[phidgetId].Enqueue(posTrimmed);
                        AxisPositionSum[phidgetId] += posTrimmed;
                    }
                    else
                    {
                        if (Math.Abs(posTrimmed * 10 - AxisPositionSum[phidgetId]) > 1000)
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
                    AxisPosition[phidgetId] =  AxisPositionSum[phidgetId] / 10;
                    //System.Diagnostics.Trace.WriteLine( m.ToString() + " " + sp.PortName + " " + (motorTrimmed + motorShift) + " " + motorTrimmed+ " " + posTrimmed);
                }
            }
        }
    }
}
