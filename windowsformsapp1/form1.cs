using System;
using System.Collections.Generic;
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
        Dictionary<int, int> TargetLoads;
        Dictionary<int, int> MotorsCPort;
        Dictionary<int, int[]> AxisLimits;
        List<SerialPort> listofPorts = new List<SerialPort>();
        List<TextBox> listOfEdits = new List<TextBox>();

        PhidgetReciver phidgetReciver = new PhidgetReciver();

        public class PhidgetReciver
        {
            Dictionary<int, int> hashMap;
            private List<InputInfo> listOfPhidgets = new List<InputInfo>();
            private Manager phidgetManager = new Manager();

            public bool abort { get; set; }
            public double getValueOf(int index)
            {
                return listOfPhidgets[index].Value;
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
                    listOfPhidgets[i].Input.DataInterval = listOfPhidgets[i].Input.MinDataInterval;
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
                initializePhidgetsControl();
                System.Diagnostics.Trace.WriteLine("PhidgetReciver started");
                while (abort)
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
                   /* if (targetHash == hashMap[0])
                    {
                        currentLoad1.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[1])
                    {
                        currentLoad2.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[2])
                    {
                        currentLoad3.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[3])
                    {
                        currentLoad4.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[4])
                    {
                        currentLoad5.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[5])
                    {
                        currentLoad6.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[6])
                    {
                        currentLoad7.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[7])
                    {
                        currentLoad8.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[8])
                    {
                        currentLoad9.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[9])
                    {
                        currentLoad10.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[10])
                    {
                        currentLoad11.Text = vr.ToString("#.##");
                    }
                    else if (targetHash == hashMap[11])
                    {
                        currentLoad12.Text = vr.ToString("#.##");
                    }
                    else
                    {
                        throw new NotImplementedException("wrongHash");
                    }*/
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

        class TestSetup
        {
            public int ArduinoPort { get; set; }
            public InputInfo PhidgetBridge { get; set; }
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
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Phidget.InvokeEventCallbacks = true;
            initializeArduinoDevices();
            Thread PhidgetTread = new Thread(new ThreadStart(phidgetReciver.ThreadRun));
            System.Diagnostics.Trace.WriteLine("Before start thread");
            PhidgetTread.Name = "PhidgetTread";
            listOfEdits.Add(currentLoad1);
            
        }


        bool initializeArduinoDevices()
        {
            // initialize arduino device list
            var devices = OS.GetSerialDevices();
            if (devices.Count > 0)
            {
                foreach (var device in devices)
                {
                    arduinoDevices.Items.Add(device);
                }
            }
            else
            {
                arduinoDevices.Items.Add(new ArduinoInfo("No devices found", ""));
            }
            arduinoDevices.SelectedIndex = 0;
            return true;
        }





        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopButton_Click(null,null);
        }
       

        private void StartButton_Click(object sender, EventArgs e)
        {
            phidgetReciver.Start();

            for (int i = 0; i < phidgetReciver.Count(); i++)
            {
                TestSetup testSetup = new TestSetup();
                testSetup.ArduinoPort = MotorsCPort[i];
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
            int lastPort = 0;
            for(int i = 0; i < MotorsCPort.Count; i++)
            {
                if (lastPort < MotorsCPort[i])
                {
                    lastPort = MotorsCPort[i];
                    SerialPort serialPort = new SerialPort();
                    listofPorts.Add(serialPort);
                    listofPorts[i].BaudRate = 115200;
                    listofPorts[i].PortName = "COM"+MotorsCPort[i];
                    listofPorts[i].DataReceived += SerialPort_DataReceived;    
                    listofPorts[i].ErrorReceived += SerialPort_ErrorReceived;
                    listofPorts[i].DtrEnable = true;
                    listofPorts[i].Open();
                    listofPorts[i].ReadExisting();
                }
            }
            StartButton.Enabled = false;
            StopButton.Enabled = true;
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

                System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"P(\d+)}");
                var match = re.Match(data);
                if (match.Success)
                {
                    for (int ctr = 1; ctr < match.Groups.Count; ctr++)
                    {
                        position = int.Parse(match.Groups[ctr].Value);
                        currentPosition.Invoke(
                            (MethodInvoker)(() => currentPosition.Text = match.Groups[ctr].Value));

                    }
                }

            }

            //System.Diagnostics.Trace.WriteLine("SP: " + match.Captures[0].Value);
            //serialPort.Write("{M0,F,100}");
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listOfSetups.Count; i++)
            {
                var command = String.Format(listOfSetups[i].MoveCommand, "B", 0);
                listofPorts[MotorsCPort[i]].Write("{" + command + "}");
            }
            for (int i = 0; i < listofPorts.Count; i++)
            {
                listofPorts[MotorsCPort[i]].Close();
            }
            // System.Threading.Thread.Sleep(500);


            StartButton.Enabled = true;
            StopButton.Enabled = false;
        }
    }
}
