using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidget22;
using Phidget22.Events;
using System.Windows.Forms;
using System.Threading;
namespace WindowsFormsApp1
{
    public class PhidgetReciver
    {
        int PipeSize = 50;
        Dictionary<int, int> hashMap;
        private Dictionary<int,InputInfo> listOfPhidgets = new Dictionary<int, InputInfo>();
        Dictionary<int, double> AxisPressSum = new Dictionary<int, double>();
        Dictionary<int, Queue<double>> AxisPressQueue = new Dictionary<int, Queue<double>>();
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
                listOfPhidgets[i].Input.VoltageRatioChange += Input_VoltageRatioChange;
                listOfPhidgets[i].Input.Detach += Input_Detach;
                listOfPhidgets[i].Input.PropertyChange += Input_PropertyChange;
                listOfPhidgets[i].Input.SensorChange += Input_SensorChange;
                listOfPhidgets[i].Input.DataInterval = listOfPhidgets[i].Input.MinDataInterval;//????????
                                                                                               //listOfPhidgets[i].Input.VoltageRatioChangeTrigger = listOfPhidgets[i].Input.MinDataInterval;//????????
                listOfPhidgets[i].Input.VoltageRatioChangeTrigger = 0;
                listOfPhidgets[i].Input.BridgeGain = BridgeGain.Gain_128x;
                listOfPhidgets[i].Input.BridgeEnabled = true;

                AxisPressSum.Add(i, 0);
                Queue<double> PressPipe = new Queue<double>();
                AxisPressQueue.Add(i, PressPipe);
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

            if (hashMap == null) hashMap = new Dictionary<int, int>();
            hashMap.Add(0, 4659180);
            hashMap.Add(1, 4659181);
            hashMap.Add(2, 4659182);
            hashMap.Add(3, 4659183);
            hashMap.Add(4, 4659282);
            hashMap.Add(5, 4659283);
            hashMap.Add(6, 4734702);
            hashMap.Add(7, 4734703);
            hashMap.Add(8, 4659280);
            hashMap.Add(9, 4659281);
            hashMap.Add(10, 4734700);
            hashMap.Add(11, 4734701);
            
            phidgetManager.Attach += OnPhidgetAttached;
            //phidgetManager.Detach += OnPhidgetDetached;

            try
            {
                phidgetManager.Open();
            }
            catch (PhidgetException error)
            {
                MessageBox.Show("Error opening Phidget Manager\n" + error.Description);
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
            input.Open(0);
            input.BridgeEnabled = false;



            var info = new InputInfo(input);
            int targetHash = input.DeviceSerialNumber *10 +input.Channel;
            int i = 0;
            for (; hashMap[i] != targetHash; i++)
            {
                if (i >= hashMap.Count) throw new NotImplementedException("wrongHash");
            }
            listOfPhidgets.Add(i, new InputInfo(input));
            System.Diagnostics.Trace.WriteLine("PhidgetReciver OnPhidgetAttached " + input.DeviceSerialNumber + " " + input.Channel);
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
            //var info = new InputInfo(args.Channel as VoltageRatioInput);
            //listOfPhidgets.Remove(info);
        }

        private void Input_VoltageRatioChange(object sender, VoltageRatioInputVoltageRatioChangeEventArgs e)
        {
            // update displayed value
            var vr = e.VoltageRatio * 1000000;
            var tmp = sender as Phidget;
            int targetHash = tmp.DeviceSerialNumber*10+tmp.Channel;
            int i = 0;
            for (; hashMap[i] != targetHash; i++)
            {
                if (i >= hashMap.Count) throw new NotImplementedException("wrongHash");
            }

            if (AxisPressQueue[i].Count < PipeSize)
            {
                AxisPressQueue[i].Enqueue(vr);
                AxisPressSum[i] += vr;
            }
            else
            {
                if (Math.Abs(vr * PipeSize - AxisPressSum[i]) > 10000000000)
                {
                    AxisPressQueue[i].Enqueue(AxisPressSum[i] / 10);
                    AxisPressSum[i] += (AxisPressSum[i] / 10);
                }
                else
                {
                    AxisPressQueue[i].Enqueue(vr);
                    AxisPressSum[i] += vr;
                }
                AxisPressSum[i] -= AxisPressQueue[i].Dequeue();
            }
            listOfPhidgets[i].value = AxisPressSum[i] / PipeSize;


        }
    }
}
