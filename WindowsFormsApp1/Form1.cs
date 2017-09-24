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
        int position = 0;
        Dictionary<int, int[]> AxisLimits;
        List<TextBox> listOfLEdits = new List<TextBox>();
        List<TextBox> listOfPEdits = new List<TextBox>();

        PhidgetReciver phidgetReciver = new PhidgetReciver();
        ArduinoReciver arduinoReciver = new ArduinoReciver();

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
                    listOfPhidgets[i].Input.BridgeEnabled = false;
                }
            }
            public void ThreadRun()
            {
                Phidget.InvokeEventCallbacks = true;
                initializePhidgetsControl();
                System.Diagnostics.Trace.WriteLine("PhidgetReciver started");
                while (!abort)
                {
                    Thread thr = Thread.CurrentThread;
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

                if (firstPick)
                {
                    zero = vr;
                    firstPick = false;
                }
                var powerLimit = 50;
                var step = 1;

                // request position
                /* serialPort.Write(testSetup.PositionRequestCommand);
                    var diff = (int)Math.Round(testSetup.TargetLoad - vr+zero);
            */
                //var diff2 = (int)Math.Round(testSetup.LastRead - vr);

                //testSetup.LastRead = (int)vr;
                // testSetup.Torque += (int)diff /*testSetup.Koeff*/;


                /*using (var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv"))
                {
                    sw.WriteLine(log);
                }*/
                /*
                testSetup.Torque += diff / 10;
                if (testSetup.Torque > powerLimit)
                {
                    testSetup.Torque = powerLimit;
                }
                else if (testSetup.Torque < (-1 * powerLimit))
                {
                    testSetup.Torque = (-1 * powerLimit);
                }

                if (testSetup.Torque > 0)   // go back
                {
                    direction = 'B';
                }
                else if (testSetup.Torque < 0) // go forward
                {
                    direction = 'F';
                }
                else
                {
                    direction = 'S';
                }*/
                /*
                if (direction != 'S')
                {
                    var torque = Math.Abs(testSetup.Torque);
                    //  var log = String.Format("{0}\t\t{1}:{3}\t\t{2}", vr, torque, diff, direction);
                    //   System.Diagnostics.Trace.WriteLine(log);

                    var command = String.Format(testSetup.MoveCommand, direction, torque);
                    if (serialPort.IsOpen)
                    { 
                        serialPort.Write("{P0}");
                    }

                    currentTorque.Text = testSetup.Torque.ToString();
                    currentPosition.Text = position.ToString();

                    if (position > int.Parse(maxPos.Text))
                    {
                        serialPort.Write("{M0,F," + ((position - int.Parse(maxPos.Text)) * 1).ToString() + "}");
                    }
                    else if (position < int.Parse(minPos.Text))
                    {
                        serialPort.Write("{M0,B," + ((int.Parse(minPos.Text) - position) * 1).ToString() + "}");
                    }
                    else
                    {
                        serialPort.Write("{" + command + "}");
                    }
                }
                */
            }
        }
        public class ArduinoReciver
        {
            Dictionary<int, int> MotorsCPort = new Dictionary<int, int>();
            Dictionary<int, int> AxisPosition = new Dictionary<int, int>();
            List<SerialPort> listofPorts = new List<SerialPort>();

            public bool abort { get; set; }

            public double getValueOf(int index)
            {
                return AxisPosition[index];
            }
            public int getMotorPort(int index)
            {
                return MotorsCPort[index];
            }
            public int Count()
            {
                return AxisPosition.Count;
            }
            public void Start()
            {
                int lastPort = 0;
                for (int i = 0; i < MotorsCPort.Count; i++)
                {
                    if (lastPort < MotorsCPort[i])
                    {
                        lastPort = MotorsCPort[i];
                        SerialPort serialPort = new SerialPort();
                        listofPorts.Add(serialPort);
                        listofPorts[i].BaudRate = 115200;
                        listofPorts[i].PortName = "COM" + MotorsCPort[i];
                        listofPorts[i].DataReceived += SerialPort_DataReceived;
                        listofPorts[i].ErrorReceived += SerialPort_ErrorReceived;
                        listofPorts[i].DtrEnable = true;
                        listofPorts[i].Open();
                        listofPorts[i].ReadExisting();
                    }
                }
            }
            public void Stop()
            {
                for (int i = 0; i < listofPorts.Count; i++)
                {
                    listofPorts[i].Close();
                }
            }
            bool initializeArduinoDevices()
            {
                for (int i = 0; i < 12; i++)
                {
                    MotorsCPort.Add(i,4);
                }
                for (int i = 0; i < MotorsCPort.Count; i++)
                {
                    AxisPosition.Add(i, 0);
                }
                return true;
            }
            public void ThreadRun()
            {
                initializeArduinoDevices();
                System.Diagnostics.Trace.WriteLine("ArduinoReciver started");
                Thread.Sleep(1000);
                while (!abort)
                {
                    Thread thr = Thread.CurrentThread;
                    System.Diagnostics.Trace.WriteLine("ArduinoReciver is ok");
                    for (int i = 0; i < listofPorts.Count; i++)
                    {
                        listofPorts[i].Write("{M0,F,0}");
                        listofPorts[i].Write("{M1,F,0}");
                        listofPorts[i].Write("{M2,F,0}");
                        listofPorts[i].Write("{M3,F,0}");
                        listofPorts[i].Write("{M4,F,0}");
                        listofPorts[i].Write("{M5,F,0}");
                    }
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
                        AxisPosition[int.Parse(tmp[0].Remove(0, 2))] = int.Parse(tmp[1].TrimEnd('}'));
                    }
                }
            }

        }


        class TestSetup
        {
            public int ArduinoPort { get; set; }
            public int Phidget { get; set; }
            public int MotorId { get; set; }
            public double TargetLoad { get; set; }
            public int MinPos { get; set; }
            public int MaxPos { get; set; }

            public int Torque { get; set; }
            public int Koeff { get; set; }
            public double LastRead { get; set; }
            public int LastDiff { get; set; }
            public char Direction { get; set; }
            public string PositionRequestCommand { get; set; }
            public string MoveCommand { get; set; }
            public bool FirstPick { get;  set; }
        }

        List<TestSetup> listOfSetups = new List<TestSetup>();


        public Form1()
        {

            for (int i = 0; i < 12; i++)
            {
                if (AxisLimits == null) AxisLimits = new Dictionary<int, int[]>();
                int[] tmp = new int[2] { 0, 1000 };
                AxisLimits.Add(i, tmp);
            }
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Thread PhidgetTread = new Thread(new ThreadStart(phidgetReciver.ThreadRun));
            Thread ArduinoTread = new Thread(new ThreadStart(arduinoReciver.ThreadRun));
            System.Diagnostics.Trace.WriteLine("Before start thread");
            PhidgetTread.Name = "PhidgetTread";
            ArduinoTread.Name = "ArduinoTread";

            phidgetReciver.abort = false;
            arduinoReciver.abort = false;
            PhidgetTread.Start();
            ArduinoTread.Start();
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

        }

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopButton_Click(null,null);
        }
       

        private void StartButton_Click(object sender, EventArgs e)
        {
            phidgetReciver.Start();
            arduinoReciver.Start();

            for (int i = 0; i < phidgetReciver.Count(); i++)
            {
                TestSetup testSetup = new TestSetup();
                testSetup.ArduinoPort = arduinoReciver.getMotorPort(i);
                testSetup.MotorId = i;
                testSetup.TargetLoad = 0;
                testSetup.LastDiff = 0;
                testSetup.LastRead = testSetup.TargetLoad;
                testSetup.MinPos = AxisLimits[i][0];
                testSetup.MaxPos = AxisLimits[i][1];
                testSetup.FirstPick = true;
                testSetup.PositionRequestCommand = "{P" + testSetup.MotorId.ToString() + "}";
                testSetup.MoveCommand = "M" + testSetup.MotorId.ToString() + ",{0},{1}";
                testSetup.Torque = 0;
                testSetup.Direction = 'S';
                testSetup.Koeff = 1;
                listOfSetups.Add(testSetup);
            }


            
            // configure serial port

            StartButton.Enabled = false;
            StopButton.Enabled = true;
            while (true)
            {
                for (int i = 0; i < phidgetReciver.Count(); i++)
                {
                    listOfLEdits[i].Text = phidgetReciver.getValueOf(i).ToString("#.##");
                }
                for (int i = 0; i < arduinoReciver.Count(); i++)
                {
                    listOfPEdits[i].Text = arduinoReciver.getValueOf(i).ToString("#.##");
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }



       

        private void StopButton_Click(object sender, EventArgs e)
        {
            phidgetReciver.Stop();
            arduinoReciver.Stop();
            for (int i = 0; i < listOfSetups.Count; i++)
            {
                var command = String.Format(listOfSetups[i].MoveCommand, "B", 0);
                //arduinoReciver.
                //listofPorts[MotorsCPort[i]].Write("{" + command + "}");
            }

            // System.Threading.Thread.Sleep(500);


            StartButton.Enabled = true;
            StopButton.Enabled = false;
        }
    }
}
