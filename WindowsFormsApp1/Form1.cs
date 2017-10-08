using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phidget22;
using Phidget22.Events;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int lastPort = 0;
        bool abort = false;
        Dictionary<int, int> MotorsCPort = new Dictionary<int, int>();
        Dictionary<int, int> MtrToPh = new Dictionary<int, int>();
        Dictionary<int, int> PhToMtr = new Dictionary<int, int>();
        List<TextBox> listOfLEdits = new List<TextBox>();
        List<TextBox> listOfPEdits = new List<TextBox>();
        List<TextBox> listOfTEdits = new List<TextBox>();
        List<TextBox> listOfSEdits = new List<TextBox>();
        List<SerialPort> listofPorts = new List<SerialPort>();

        Thread PhidgetTread;
        Thread ReciverTread;
        Thread SenderTread;
        Thread CalculatorTread;
        Thread UDPReceiverTread;

        PhidgetReciver phidgetReciver = new PhidgetReciver();
        MotionCalculator motionCalculator = new MotionCalculator();
        ArduinoReciver arduinoReciver;
        ArduinoSender arduinoSender;
        UdpReceiver udpReceiver;

        public class PhidgetReciver
        {
            Dictionary<int, int> hashMap;
            private List<InputInfo> listOfPhidgets = new List<InputInfo>();
            private Manager phidgetManager = new Manager();

            public bool abort { get; set; }
            public double getValueOf(int index)
            {
                return listOfPhidgets[index].value;
            }
            public int Count()
            {
                return listOfPhidgets.Count;
            }
            public void Start()
            {
                // setup phidget bridge events
                for (int i = 0; i < listOfPhidgets.Count; i++)
                {
                    listOfPhidgets[i].Input.DataInterval = listOfPhidgets[i].Input.MinDataInterval;//????????
                    //listOfPhidgets[i].Input.VoltageRatioChangeTrigger = listOfPhidgets[i].Input.MinDataInterval;//????????
                    listOfPhidgets[i].Input.VoltageRatioChangeTrigger = 0;
                    listOfPhidgets[i].Input.BridgeGain = BridgeGain.Gain_128x;
                    listOfPhidgets[i].Input.BridgeEnabled = true;
                }
            }
            public void Stop()
            {
                // setup phidget bridge events
                for (int i = 0; i < listOfPhidgets.Count; i++)
                {
                    //listOfPhidgets[i].Input.BridgeEnabled = false;
                }
            }
            public void ThreadRun()
            {
                Phidget.InvokeEventCallbacks = true;
                initializePhidgetsControl();
                System.Diagnostics.Trace.WriteLine("PhidgetReciver started");
                while (!abort)
                {
                    //System.Diagnostics.Trace.WriteLine("PhidgetReciver is ok");
                    Thread.Sleep(1000);
                }
                phidgetManager.Close();
                System.Diagnostics.Trace.WriteLine("PhidgetReciver stopped");
            }
            bool initializePhidgetsControl()
            {
                phidgetManager.Attach += OnPhidgetAttached;
                //phidgetManager.Detach += OnPhidgetDetached;

                try
                {
                    phidgetManager.Open();
                }
                catch (PhidgetException error)
                {
                    MessageBox.Show( "Error opening Phidget Manager\n" + error.Description);
                    return false;
                }
                return true;
            }
            void OnPhidgetAttached(object sender, ManagerAttachEventArgs args)
            {
                if (hashMap == null) hashMap = new Dictionary<int, int>();
                // add device to the list
                VoltageRatioInput input = new VoltageRatioInput();
                input.DeviceSerialNumber = args.Channel.DeviceSerialNumber;
                input.Channel = args.Channel.Channel;
                input.DeviceLabel = Phidget.AnyLabel;
                input.HubPort = Phidget.AnyHubPort;
                input.Error += Input_Error;
                input.VoltageRatioChange += Input_VoltageRatioChange;
                input.Detach += Input_Detach;
                input.PropertyChange += Input_PropertyChange;
                input.SensorChange += Input_SensorChange;

                input.Open(0);
                input.BridgeEnabled = false;

                hashMap.Add(hashMap.Count, input.GetHashCode());
                var info = new InputInfo(input);
                listOfPhidgets.Add(new InputInfo(input));
                System.Diagnostics.Trace.WriteLine("PhidgetReciver OnPhidgetAttached "+ input.DeviceSerialNumber +" " + input.Channel);
            }
            private void Input_PropertyChange(object sender, PropertyChangeEventArgs e)
            {
                throw new NotImplementedException();
            }

            private void Input_Detach(object sender, DetachEventArgs e)
            {
                throw new NotImplementedException();
            }

            private void Input_Attach(object sender, AttachEventArgs e)
            {
                // add device to the list
            }
            private void Input_Error(object sender, ErrorEventArgs e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }
            private void Input_SensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
            {
                throw new NotImplementedException();
            }

            void OnPhidgetDetached(object obj, ManagerDetachEventArgs args)
            {
                var info = new InputInfo(args.Channel as VoltageRatioInput);
                listOfPhidgets.Remove(info);
            }

            private void Input_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
            {
                // update displayed value
                var vr = e.VoltageRatio * 1000000;
                
                var targetHash = sender.GetHashCode();
                int i = 0;
                for (; hashMap[i] != targetHash ; i++)
                {
                    if (i >= hashMap.Count) throw new NotImplementedException("wrongHash");
                }
                    
                listOfPhidgets[i].value = vr;


            }
        }
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
                int motorShift = (i-1) * 6;
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
                        if (phidgetId == 11)
                        {
                           System.Diagnostics.Trace.WriteLine(/*m.ToString() + " " + " " + */AxisPositionSum[phidgetId] +"," + posTrimmed + "," + AxisPosition[phidgetId]);
                           // System.Diagnostics.Trace.WriteLine(m.ToString() + " " + sp.PortName + " " + (motorTrimmed + motorShift) + " " + motorTrimmed + " " + posTrimmed);
                        }
                        
                        if (AxisPositionQueue[phidgetId].Count < 10)
                        {
                            AxisPositionQueue[phidgetId].Enqueue(posTrimmed);
                            AxisPositionSum[phidgetId] += posTrimmed;
                        }else
                        {
                            if (Math.Abs(posTrimmed*10 - AxisPositionSum[phidgetId]) > 100)
                            {
                                AxisPositionQueue[phidgetId].Enqueue(AxisPositionSum[phidgetId]/10);
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

            public void setValueOf(int index,int val,char dir)
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
                        //if (i == 11)
                        //{
                        //    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                        //}
                        //1if (i == 5)
                        //1{
                        //1    tmp = ("{M" + PhToMtr[i] % 6 + "," + AxisTorque[i].Direction + "," + Math.Abs(AxisTorque[i].Torque).ToString() + "}");
                        //1}
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
        public class MotionCalculator
        {


            Dictionary<int, int[]> AxisLimits;
            List<TestSetup> listOfSetups = new List<TestSetup>();

            public bool abort { get; set; }

            public TestSetup getSetupAt(int index)
            {
                return listOfSetups[index];
            }
            public int Count()
            {
                return listOfSetups.Count;
            }
            public void Start()
            {
            }
            public void Stop()
            {
            }
            bool initializeCalculator()
            {
                for (int i = 0; i < 12; i++)
                {
                    if (AxisLimits == null) AxisLimits = new Dictionary<int, int[]>();
                    int[] tmp = new int[2] { 0, 5000 };
                    AxisLimits.Add(i, tmp);
                }
                AxisLimits[11][0] = 200;
                AxisLimits[11][1] = 550;
                for (int i = 0; i < AxisLimits.Count; i++)
                {
                    TestSetup testSetup = new TestSetup();
                    testSetup.MotorId = i;
                    testSetup.TargetLoad = 0;
                    testSetup.LastDiff = 0;
                    testSetup.LastLoad = testSetup.TargetLoad;
                    testSetup.Load = testSetup.TargetLoad;
                    testSetup.ZeroLoad = testSetup.TargetLoad;
                    testSetup.MinPos = AxisLimits[i][0];
                    testSetup.MaxPos = AxisLimits[i][1];
                    testSetup.FirstPick = true;
                    testSetup.PositionRequestCommand = "{P" + testSetup.MotorId.ToString() + "}";
                    testSetup.MoveCommand = "M" + testSetup.MotorId.ToString() + ",{0},{1}";
                    testSetup.Torque = 0;
                    testSetup.TorqueLimit = 100;
                    testSetup.Koeff = 0.03;
                    testSetup.Direction = 'S';
                    listOfSetups.Add(testSetup);
                }

                return true;
            }
            public void ThreadRun()
            {
                
                initializeCalculator();
                System.Diagnostics.Trace.WriteLine("MotionCalculator started");
                while (listOfSetups[0].Load == 0 || listOfSetups[listOfSetups.Count-1].Load == 0) ;
                for (int j = 0; j < 100; j++)
                {
                    for (int i = 0; i < listOfSetups.Count; i++)
                    {
                        listOfSetups[i].ZeroLoad += listOfSetups[i].Load;
                    }
                }
                for (int i = 0; i < listOfSetups.Count; i++)
                {
                    listOfSetups[i].ZeroLoad = listOfSetups[i].ZeroLoad / 100;
                }
                while (!abort)
                {
                    //System.Diagnostics.Trace.WriteLine("MotionCalculator is ok");
                    Thread.Sleep(10);
                    for (int i = 0; i < listOfSetups.Count; i++)
                    {
                        // request position
                        double diff = (listOfSetups[i].TargetLoad + listOfSetups[i].Load - listOfSetups[i].ZeroLoad);
                        listOfSetups[i].LastDiff += Math.Pow(diff, 1);
                        /*using (var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv"))
                        {
                            sw.WriteLine(log);
                        }*/
                        double limitSum = (double)listOfSetups[i].TorqueLimit / listOfSetups[i].Koeff;
                        if (listOfSetups[i].LastDiff > limitSum)
                        {
                            listOfSetups[i].LastDiff = limitSum;
                        }
                        if (listOfSetups[i].LastDiff < -limitSum)
                        {
                            listOfSetups[i].LastDiff = -limitSum;
                        }
                        if (listOfSetups[i].MinPos > listOfSetups[i].Position)
                        {
                            listOfSetups[i].Torque = -40;listOfSetups[i].LastDiff = 0;
                        }
                        else if (listOfSetups[i].MaxPos < listOfSetups[i].Position)
                        {
                            listOfSetups[i].Torque = 40; listOfSetups[i].LastDiff = 0;
                        }
                        else listOfSetups[i].Torque = (int)Math.Round(listOfSetups[i].LastDiff * listOfSetups[i].Koeff);

                        if (listOfSetups[i].Torque > listOfSetups[i].TorqueLimit)
                        {
                            listOfSetups[i].Torque = listOfSetups[i].TorqueLimit;
                        }
                        else if (listOfSetups[i].Torque < (-1 * listOfSetups[i].TorqueLimit))
                        {
                            listOfSetups[i].Torque = (-1 * listOfSetups[i].TorqueLimit);
                        }
                        if (listOfSetups[i].Torque > 0)   // go back
                        {
                            listOfSetups[i].Direction = 'B';
                        }
                        else if (listOfSetups[i].Torque < 0) // go forward
                        {
                            listOfSetups[i].Direction = 'F';
                        }
                        else
                        {
                            listOfSetups[i].Direction = 'F';
                        }
                    }
                }
                System.Diagnostics.Trace.WriteLine("MotionCalculator stopped");
            }
        }



        public Form1()
        {
            for (int i = 0; i < 6; i++)
            {
                MotorsCPort.Add(i,4);
            }
            for (int i = 6; i < 12; i++)
            {
                MotorsCPort.Add(i, 8);
            }
            {
                MtrToPh.Add( 0,10);
                MtrToPh.Add( 1,11);
                MtrToPh.Add( 2,4);
                MtrToPh.Add( 3,5);
                MtrToPh.Add( 4,6);
                MtrToPh.Add( 5,7);
                MtrToPh.Add( 6,0);
                MtrToPh.Add( 7,1);
                MtrToPh.Add( 8,2);
                MtrToPh.Add( 9,3);
                MtrToPh.Add( 10,8);
                MtrToPh.Add( 11,9);
            }
            for (int i = 0; i < MtrToPh.Count; i++)
            {
                PhToMtr.Add(MtrToPh[i],i);
            }
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < MotorsCPort.Count; i++)
            {
                if (lastPort < MotorsCPort[i])
                {
                    lastPort = MotorsCPort[i];
                    SerialPort serialPort = new SerialPort();
                    serialPort.BaudRate = 115200;
                    serialPort.PortName = "COM" + MotorsCPort[i];
                    serialPort.WriteTimeout = 1000;
                    serialPort.DtrEnable = false;
                    serialPort.RtsEnable = false;
                    serialPort.Open();


                    serialPort.Write("{M0,F,0}{M1,F,0}{M2,F,0}{M3,F,0}{M4,F,0}{M5,F,0}");
                    serialPort.ReadExisting();
                    listofPorts.Add(serialPort);
                }
            }
            UdpClient udpClient = new UdpClient(8888);


            abort = false;
            arduinoReciver = new ArduinoReciver(ref listofPorts,ref MtrToPh);
            arduinoSender = new ArduinoSender(ref listofPorts, ref PhToMtr);
            udpReceiver = new UdpReceiver(ref udpClient);
            PhidgetTread = new Thread(new ThreadStart(phidgetReciver.ThreadRun));
            ReciverTread = new Thread(new ThreadStart(arduinoReciver.ThreadRun));
            SenderTread = new Thread(new ThreadStart(arduinoSender.ThreadRun));
            CalculatorTread = new Thread(new ThreadStart(motionCalculator.ThreadRun));
            UDPReceiverTread = new Thread(new ThreadStart(udpReceiver.ThreadRun));
            System.Diagnostics.Trace.WriteLine("Before start thread");
            PhidgetTread.Name = "PhidgetTread";
            ReciverTread.Name = "ReciverTread";
            SenderTread.Name = "SenderTread";
            CalculatorTread.Name = "CalculatorTread";
            UDPReceiverTread.Name = "UDPReceiverTread";

            phidgetReciver.abort = false;
            arduinoReciver.abort = false;
            arduinoSender.abort = false;
            motionCalculator.abort = false;
            PhidgetTread.Start();
            ReciverTread.Start();
            SenderTread.Start();
            CalculatorTread.Start();
            UDPReceiverTread.Start();
            {
                listOfLEdits.Add(currentLoad1);
                listOfLEdits.Add(currentLoad2);
                listOfLEdits.Add(currentLoad3);
                listOfLEdits.Add(currentLoad4);
                listOfLEdits.Add(currentLoad5);
                listOfLEdits.Add(currentLoad6);
                listOfLEdits.Add(currentLoad7);
                listOfLEdits.Add(currentLoad8);
                listOfLEdits.Add(currentLoad9);
                listOfLEdits.Add(currentLoad10);
                listOfLEdits.Add(currentLoad11);
                listOfLEdits.Add(currentLoad12);
            }
            {
                listOfPEdits.Add(currPos1);
                listOfPEdits.Add(currPos2);
                listOfPEdits.Add(currPos3);
                listOfPEdits.Add(currPos4);
                listOfPEdits.Add(currPos5);
                listOfPEdits.Add(currPos6);
                listOfPEdits.Add(currPos7);
                listOfPEdits.Add(currPos8);
                listOfPEdits.Add(currPos9);
                listOfPEdits.Add(currPos10);
                listOfPEdits.Add(currPos11);
                listOfPEdits.Add(currPos12);
            }
            {
                listOfTEdits.Add(currTorque1);
                listOfTEdits.Add(currTorque2);
                listOfTEdits.Add(currTorque3);
                listOfTEdits.Add(currTorque4);
                listOfTEdits.Add(currTorque5);
                listOfTEdits.Add(currTorque6);
                listOfTEdits.Add(currTorque7);
                listOfTEdits.Add(currTorque8);
                listOfTEdits.Add(currTorque9);
                listOfTEdits.Add(currTorque10);
                listOfTEdits.Add(currTorque11);
                listOfTEdits.Add(currTorque12);
            }
            {
                listOfSEdits.Add(totalDiff1);
                listOfSEdits.Add(totalDiff2);
                listOfSEdits.Add(totalDiff3);
                listOfSEdits.Add(totalDiff4);
                listOfSEdits.Add(totalDiff5);
                listOfSEdits.Add(totalDiff6);
                listOfSEdits.Add(totalDiff7);
                listOfSEdits.Add(totalDiff8);
                listOfSEdits.Add(totalDiff9);
                listOfSEdits.Add(totalDiff10);
                listOfSEdits.Add(totalDiff11);
                listOfSEdits.Add(totalDiff12);
            }

        }

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopButton_Click(null,null);
            phidgetReciver.abort = true;
            arduinoReciver.abort = true;
            arduinoSender.abort = true;
            motionCalculator.abort = true;
            udpReceiver.Stop();
            abort = true;
            if(PhidgetTread.IsAlive)PhidgetTread.Join();
            if(ReciverTread.IsAlive)ReciverTread.Join();
            if(SenderTread.IsAlive)SenderTread.Join();
            if (CalculatorTread.IsAlive) CalculatorTread.Join();
            //if (UDPReceiverTread.IsAlive) UDPReceiverTread.Join();
            for (int j = 0; j < listofPorts.Count; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    string tmp = ("{M" + i + ",F,0}");
                    listofPorts[j].Write(tmp);
                    listofPorts[j].ReadExisting();
                }
            }
            Thread.Sleep(100);
            for (int i = 0; i < listofPorts.Count; i++)
            {
                listofPorts[i].Close();
            }
        }
       

        private void StartButton_Click(object sender, EventArgs e)
        {
            phidgetReciver.Start();
            arduinoReciver.Start();
            arduinoSender.Start();
            motionCalculator.Start();
            StartButton.Enabled = false;
            StopButton.Enabled = true;
            var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv");

            while (!abort)
            {
                for (int i = 0; i < phidgetReciver.Count(); i++)
                {
                    listOfLEdits[i].Text = (motionCalculator.getSetupAt(i).ZeroLoad -phidgetReciver.getValueOf(i)).ToString("#.##");
                    listOfPEdits[i].Text = arduinoReciver.getValueOf(i).ToString("#.##");
                    listOfTEdits[i].Text = motionCalculator.getSetupAt(i).Direction + motionCalculator.getSetupAt(i).Torque.ToString("#.##");
                    listOfSEdits[i].Text = motionCalculator.getSetupAt(i).LastDiff.ToString("#.##");
                    motionCalculator.getSetupAt(i).Load = phidgetReciver.getValueOf(i);
                    motionCalculator.getSetupAt(i).Position = arduinoReciver.getValueOf(i);
                    arduinoSender.setValueOf(i, motionCalculator.getSetupAt(i).Torque, motionCalculator.getSetupAt(i).Direction);
                }
                sw.WriteLine(motionCalculator.getSetupAt(2).LastDiff + "," + phidgetReciver.getValueOf(2).ToString() + "," + motionCalculator.getSetupAt(2).Torque.ToString("#.##") + "," + arduinoReciver.getValueOf(2).ToString("#.##"));
                //Thread.Sleep(10);
                Application.DoEvents();
            }
            sw.Close();

        }
        
        private void StopButton_Click(object sender, EventArgs e)
        {
            phidgetReciver.Stop();
            arduinoReciver.Stop();
            motionCalculator.Stop();
            //for (int i = 0; i < listOfSetups.Count; i++)
            //{
              //  var command = String.Format(listOfSetups[i].MoveCommand, "B", 0);
                //arduinoReciver.
                //listofPorts[MotorsCPort[i]].Write("{" + command + "}");
            //}

            // System.Threading.Thread.Sleep(500);


            StartButton.Enabled = true;
            StopButton.Enabled = false;
        }

        private void STOPButt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
    public class TestSetup
    {
        public int Phidget { get; set; }
        public int MotorId { get; set; }
        public double TargetLoad { get; set; }
        public int MinPos { get; set; }
        public int MaxPos { get; set; }

        public int Torque { get; set; }
        public int TorqueLimit { get; set; }
        public double Koeff { get; set; }
        public double LastLoad { get; set; }
        public double Load { get; set; }
        public double ZeroLoad { get; set; }
        public int Position { get; set; }
        public double LastDiff { get; set; }
        public char Direction { get; set; }
        public string PositionRequestCommand { get; set; }
        public string MoveCommand { get; set; }
        public bool FirstPick { get; set; }
    }
    public class TorqueAndDir
    {
        public int Torque { get; set; }
        public char Direction { get; set; }
    }
}
