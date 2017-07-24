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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int position = 0;


        class TestSetup
        {
            public ArduinoInfo ArduinoDevice { get; set; }
            public InputInfo PhidgetBridge { get; set; }
            public int MotorId { get; set; }
            public int ChannelId { get; set; }
            public double TargetLoad { get; set; }
            public int MinPos { get; set; }
            public int MaxPos { get; set; }

            public int Torque { get; set; }
            public int Koeff { get; set; }
            public double LastRead { get; set; }
            public int LastDiff { get; set; }

            public string PositionRequestCommand { get; set; }
            public string MoveCommand { get; set; }
        }

        private Manager phidgetManager = new Manager();
        private TestSetup testSetup = null;
        private SerialPort serialPort =  new SerialPort(); // serial port to communicate with arduino

        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void UpdateControls()
        {
            bool enableStart = true;

            if (Inputs.Items.Count > 0)
            {
                if (! Inputs.Enabled)
                {
                    Inputs.Enabled = true;
                    Inputs.SelectedIndex = 0;
                }
            }
            else
            {
                if (Inputs.Enabled)
                {
                    Inputs.SelectedIndex = -1;
                    Inputs.Enabled = false;
                    enableStart = false;
                }
            }


            // update arduino controls
            if (arduinoDevices.Items.Count > 0)
            {
                if (!arduinoDevices.Enabled)
                {
                    arduinoDevices.Enabled = true;
                    motorId.Enabled = true;

                    arduinoDevices.SelectedIndex = 0;
                    motorId.SelectedIndex = 0;
                }
            }
            else
            {
                if (arduinoDevices.Enabled)
                {
                    arduinoDevices.Enabled = false;
                    motorId.Enabled = false;

                    arduinoDevices.SelectedIndex = -1;
                    motorId.SelectedIndex = -1;

                    enableStart = false;
                }
            }

            StartButton.Enabled = enableStart;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Phidget.InvokeEventCallbacks = true;
            initializePhidgetsControl();
            initializeArduinoDevices();
        }


        bool initializeArduinoDevices ()
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

        bool initializePhidgetsControl ()
        {
            phidgetManager.Attach += OnPhidgetAttached;
            //phidgetManager.Detach += OnPhidgetDetached;
   
            try
            {
                phidgetManager.Open();
            }
            catch (PhidgetException error)
            {
                MessageBox.Show(this, "Error opening Phidget Manager\n" + error.Description);
                return false;
            }
            return true;
        }

        void OnPhidgetAttached(object sender, ManagerAttachEventArgs args)
        {
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

            var info = new InputInfo(input);
            Inputs.Items.Add(info);

            UpdateControls();
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

        void OnPhidgetDetached(object obj, ManagerDetachEventArgs args)
        {
            var info = new InputInfo(args.Channel as VoltageRatioInput);
            Inputs.Items.Remove(info);
            UpdateControls();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            phidgetManager.Close();
        }

        private void arduinoDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            testSetup = new TestSetup();

            testSetup.ArduinoDevice = arduinoDevices.SelectedItem as ArduinoInfo;

            testSetup.MotorId = motorId.SelectedIndex;
            testSetup.ChannelId = Inputs.SelectedIndex;

            testSetup.TargetLoad = double.Parse(targetLoad.Text);
            testSetup.LastDiff = 0;
            testSetup.LastRead = testSetup.TargetLoad;

            //testSetup.MinPos = int.Parse(minPos.Text);
            //testSetup.MaxPos = int.Parse(maxPos.Text);

            testSetup.PositionRequestCommand = "{P" + testSetup.MotorId.ToString() + "}";
            testSetup.MoveCommand = "M" + testSetup.MotorId.ToString() + ",{0},{1}";
            testSetup.Torque = 0;
            direction = 'S';
            testSetup.Koeff = 1;
            
            // configure serial port
            serialPort.BaudRate = 115200;
            serialPort.PortName = testSetup.ArduinoDevice.Port;
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.ErrorReceived += SerialPort_ErrorReceived;

            serialPort.Open();

            // setup phidget bridge events
            var info = Inputs.SelectedItem as InputInfo;
            info.Input.DataInterval = info.Input.MinDataInterval;
            info.Input.VoltageRatioChangeTrigger = 0;
            info.Input.BridgeGain = BridgeGain.Gain_128x;
            info.Input.BridgeEnabled = true;

            StartButton.Enabled = false;
            StopButton.Enabled = true;
        }

        private void Input_Error(object sender, ErrorEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine(e);
        }

        static long count = 0;
        static char direction = 'S';

        private void Input_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            if (testSetup != null)
            {
                ++count;
                // update displayed value
                var vr = e.VoltageRatio * 1000000;
                currentLoad.Text = vr.ToString();

                var zero = -42;
                var powerLimit = 150;
                var step = 1;

                // request position
                serialPort.Write(testSetup.PositionRequestCommand);
                var diff = (int)Math.Round(testSetup.TargetLoad - vr);
                //var diff2 = (int)Math.Round(testSetup.LastRead - vr);

                //testSetup.LastRead = (int)vr;
                // testSetup.Torque += (int)diff /*testSetup.Koeff*/;

                /*
                var torque = Math.Abs(testSetup.Torque);
                if (torque > 50)
                {
                    torque = 50;
                }*/

                /*
                using (var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv"))
                {
                    sw.WriteLine(log);
                }*/

                testSetup.Torque += diff / 10;
                if (testSetup.Torque > powerLimit)
                {
                    testSetup.Torque = powerLimit;
                }
                else if (testSetup.Torque < (-1*powerLimit))
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
                }

                if (direction != 'S')
                {
                    var torque = Math.Abs(testSetup.Torque);
                    var log = String.Format("{0}\t\t{1}:{3}\t\t{2}", vr, torque, diff, direction);
                    //System.Diagnostics.Trace.WriteLine(log);

                    var command = String.Format(testSetup.MoveCommand, direction, torque);
                    serialPort.Write("{P0}");
                    //serialPort.Write("{" + command + "}");

                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("count = " + count);
            }
        }

        private void Input_SensorChange(object sender, VoltageRatioInputSensorChangeEventArgs e)
        {
            
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            var data = sp.ReadExisting();

            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"P(\d+),");
            var match = re.Match(data);
            if (match.Success)
            {
                position = int.Parse(match.Value);
                currentPosition.Text = match.Value;
            }
           

            System.Diagnostics.Trace.WriteLine("SP: " + sp.ReadExisting());
            //serialPort.Write("{M0,F,100}");
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            var info = Inputs.SelectedItem as InputInfo;
            info.Input.BridgeEnabled = false;

            var command = String.Format(testSetup.MoveCommand, "B", 0);
            serialPort.Write("{" + command + "}");

            System.Threading.Thread.Sleep(500);
            serialPort.Close();

            StartButton.Enabled = true;
            StopButton.Enabled = false;

            testSetup = null;
        }
    }
}
