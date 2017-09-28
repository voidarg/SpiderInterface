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
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int lastPort = 0;
        bool abort = false;
        Dictionary<int, int> MotorsCPort = new Dictionary<int, int>();
        List<TextBox> listOfLEdits = new List<TextBox>();
        List<TextBox> listOfPEdits = new List<TextBox>();
        List<TextBox> listOfTEdits = new List<TextBox>();
        List<SerialPort> listofPorts = new List<SerialPort>();

        Thread PhidgetTread;
        Thread ReciverTread;
        Thread SenderTread;
        Thread CalculatorTread;

        PhidgetReciver phidgetReciver = new PhidgetReciver();
        MotionCalculator motionCalculator = new MotionCalculator();
        ArduinoReciver arduinoReciver;
        ArduinoSender arduinoSender;

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
                    System.Diagnostics.Trace.WriteLine("PhidgetReciver is ok");
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
            static long count = 0;
            static bool firstPick = true;
            static double zero = 0;
            private void Input_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
            {
                ++count;
                int curPhidget = -1;
                // update displayed value
                var vr = e.VoltageRatio * 1000000;
                
                {
                    var targetHash = sender.GetHashCode();
                    int i = 0;
                    for (; hashMap[i] != targetHash ; i++)
                    {
                        if (i >= hashMap.Count) throw new NotImplementedException("wrongHash");
                    }
                    
                    listOfPhidgets[i].value = vr;
                    Application.DoEvents();
                }


            }
        }
        public class ArduinoReciver
        {
            Dictionary<int, int> AxisPosition = new Dictionary<int, int>();
            List<SerialPort> m_listofPorts;
            public ArduinoReciver(ref List<SerialPort> _listofPorts)
            {
                m_listofPorts = _listofPorts;
            }

            public bool abort { get; set; }

            public int getValueOf(int index)
            {
                return AxisPosition[index];
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

                if (sp.IsOpen)
                {
                    var data = sp.ReadExisting();
                    MatchCollection matchMtr = Regex.Matches(data, @"{M(\d+),P(\d+)}");
                    foreach (Match m in matchMtr)
                    {
                        string[] tmp = m.ToString().Split(new string[] { ",P" }, StringSplitOptions.None);
                        System.Diagnostics.Trace.WriteLine(int.Parse(tmp[0].Remove(0, 2)) + " " + int.Parse(tmp[1].TrimEnd('}')));
                        System.Diagnostics.Trace.WriteLine(int.Parse(tmp[0].Remove(0, 2)).ToString());
                        AxisPosition[int.Parse(tmp[0].Remove(0, 2))] = int.Parse(tmp[1].TrimEnd('}'));
                    }
                }
            }

        }
        public class ArduinoSender
        {
            Dictionary<int, TorqueAndDir> AxisTorque = new Dictionary<int, TorqueAndDir>();
            Dictionary<int, char> AxisDir = new Dictionary<int, char>();
            List<SerialPort> m_listofPorts;
            public ArduinoSender(ref List<SerialPort> _listofPorts)
            {
                m_listofPorts = _listofPorts;
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
                    System.Diagnostics.Trace.WriteLine("ArduinoSender is ok:"+ AxisTorque.Count.ToString());
                    for (int i = 11; i < AxisTorque.Count; i++)
                    {
                        string tmp = ("{M" + 0 + "," + AxisTorque[i].Direction + "," + AxisTorque[i].Torque.ToString() + "}");
                        m_listofPorts[0].Write(tmp);
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
                    int[] tmp = new int[2] { 0, 1000 };
                    AxisLimits.Add(i, tmp);
                }
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
                    testSetup.TorqueLimit = 50;
                    testSetup.Direction = 'S';
                    testSetup.Koeff = 00000.1;
                    listOfSetups.Add(testSetup);
                }
                return true;
            }
            public void ThreadRun()
            {
                initializeCalculator();
                System.Diagnostics.Trace.WriteLine("MotionCalculator started");
                //Thread.Sleep(3000);
                for (int i = 0; i < listOfSetups.Count; i++)
                {
                    if (listOfSetups[i].FirstPick)
                    {
                        listOfSetups[i].ZeroLoad = listOfSetups[i].Load;
                        listOfSetups[i].FirstPick = false;
                    }
                }
                while (!abort)
                {
                    //System.Diagnostics.Trace.WriteLine("MotionCalculator is ok");
                    Thread.Sleep(10);
                    for (int i = 0; i < listOfSetups.Count; i++)
                    {
                    // request position
                        var diff = (int)Math.Round(listOfSetups[i].TargetLoad - listOfSetups[i].Load+ listOfSetups[i].ZeroLoad);
                        listOfSetups[i].LastLoad = (int)listOfSetups[i].Load;
                        listOfSetups[i].Torque += (int)diff;
                        /*using (var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv"))
                        {
                            sw.WriteLine(log);
                        }*/
                        listOfSetups[i].Torque += diff / 10;
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
                            listOfSetups[i].Direction = 'F';
                        }
                        else if (listOfSetups[i].Torque < 0) // go forward
                        {
                            listOfSetups[i].Direction = 'B';
                        }
                        else
                        {
                            listOfSetups[i].Direction = 'F';
                        }
                        listOfSetups[i].Torque = Math.Abs(listOfSetups[i].Torque);
                    }
                }
                System.Diagnostics.Trace.WriteLine("MotionCalculator stopped");
            }
        }



        public Form1()
        {
            for (int i = 0; i < 12; i++)
            {
                MotorsCPort.Add(i,3);
            }
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
//            for (int i = 0; i < MotorsCPort.Count; i++)
//            {
//                if (lastPort < MotorsCPort[i])
//                {
//                    lastPort = MotorsCPort[i];
                    SerialPort serialPort = new SerialPort();
                    listofPorts.Add(serialPort);
                    listofPorts[0].BaudRate = 115200;
                    listofPorts[0].PortName = "COM" + MotorsCPort[0];
                    listofPorts[0].WriteTimeout = 100;
                    listofPorts[0].DtrEnable = true;
                    listofPorts[0].Open();
                    listofPorts[0].ReadExisting();
//                }
//            }
            abort = false;
            arduinoReciver = new ArduinoReciver(ref listofPorts);
            arduinoSender = new ArduinoSender(ref listofPorts);
            PhidgetTread = new Thread(new ThreadStart(phidgetReciver.ThreadRun));
            ReciverTread = new Thread(new ThreadStart(arduinoReciver.ThreadRun));
            SenderTread = new Thread(new ThreadStart(arduinoSender.ThreadRun));
            CalculatorTread = new Thread(new ThreadStart(motionCalculator.ThreadRun));
            System.Diagnostics.Trace.WriteLine("Before start thread");
            PhidgetTread.Name = "PhidgetTread";
            ReciverTread.Name = "ReciverTread";
            SenderTread.Name = "SenderTread";
            CalculatorTread.Name = "CalculatorTread";

            phidgetReciver.abort = false;
            arduinoReciver.abort = false;
            arduinoSender.abort = false;
            motionCalculator.abort = false;
            PhidgetTread.Start();
            ReciverTread.Start();
            SenderTread.Start();
            CalculatorTread.Start();
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

        }

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopButton_Click(null,null);
            phidgetReciver.abort = true;
            arduinoReciver.abort = true;
            arduinoSender.abort = true;
            motionCalculator.abort = true;
            abort = true;
            PhidgetTread.Join();
            ReciverTread.Join();
            SenderTread.Join();
            CalculatorTread.Join();
            for (int i = 0; i < 12; i++)
            {
                string tmp = ("{M" + i + ",F,0}");
                listofPorts[0].Write(tmp);
            }

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
            while (!abort)
            {
                for (int i = 0; i < phidgetReciver.Count(); i++)
                {
                    listOfLEdits[i].Text = phidgetReciver.getValueOf(i).ToString("#.##");
                    listOfPEdits[i].Text = arduinoReciver.getValueOf(i).ToString("#.##");
                    listOfTEdits[i].Text = motionCalculator.getSetupAt(i).Direction + motionCalculator.getSetupAt(i).Torque.ToString("#.##");
                    motionCalculator.getSetupAt(i).Load = phidgetReciver.getValueOf(i);
                    motionCalculator.getSetupAt(i).Position = arduinoReciver.getValueOf(i);
                    arduinoSender.setValueOf(i, motionCalculator.getSetupAt(i).Torque, motionCalculator.getSetupAt(i).Direction);
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
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
        public int LastDiff { get; set; }
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
