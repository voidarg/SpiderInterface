using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
        List<TextBox> listOfKHEdits = new List<TextBox>();
        List<TextBox> listOfKVEdits = new List<TextBox>();
        List<SerialPort> listofPorts = new List<SerialPort>();

        Thread PhidgetThread;
        Thread ReciverThread;
        Thread SenderThread;
        Thread CalculatorThread;
        Thread UDPReceiverThread;
        Thread DataExchangerThread;

        PhidgetReciver phidgetReciver = new PhidgetReciver();
        MotionCalculator motionCalculator = new MotionCalculator();
        ArduinoReciver arduinoReciver;
        ArduinoSender arduinoSender;
        UdpReceiver udpReceiver;
        DataExchanger dataExchanger;

        public Form1()
        {
            string[] portnames = SerialPort.GetPortNames();
            if (portnames.Length != 3) throw new NotImplementedException();
            Array.Sort(portnames);
            for (int i = 0; i < 12; i++)
            {
                MotorsCPort.Add(i, int.Parse(portnames[i/4].Remove(0, 3)));
            }

            {
                PhToMtr.Add( 0,10); //LAL1
                PhToMtr.Add( 1,11); //LAL2
                PhToMtr.Add( 2,12); //LAL3
                PhToMtr.Add( 3,13); //LAR1
                PhToMtr.Add( 4,0);  //LLF
                PhToMtr.Add( 5,1);  //LLB
                PhToMtr.Add( 6,22);  //NC
                PhToMtr.Add( 7,23);  //NC
                PhToMtr.Add( 8,20); //LAR2
                PhToMtr.Add( 9,21); //LAR3
                PhToMtr.Add( 10,2); //RLF
                PhToMtr.Add( 11,3); //RLB
            }
            for (int i = 0; i < PhToMtr.Count; i++)
            {
                MtrToPh.Add(PhToMtr[i],i);
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
            arduinoSender = new ArduinoSender(ref listofPorts, ref PhToMtr);
            arduinoReciver = new ArduinoReciver(ref listofPorts,ref MtrToPh);
            udpReceiver = new UdpReceiver(ref udpClient);
            dataExchanger = new DataExchanger(ref arduinoReciver, ref arduinoSender, ref phidgetReciver, ref motionCalculator);
            PhidgetThread = new Thread(new ThreadStart(phidgetReciver.ThreadRun));
            ReciverThread = new Thread(new ThreadStart(arduinoReciver.ThreadRun));
            SenderThread = new Thread(new ThreadStart(arduinoSender.ThreadRun));
            CalculatorThread = new Thread(new ThreadStart(motionCalculator.ThreadRun));
            UDPReceiverThread = new Thread(new ThreadStart(udpReceiver.ThreadRun));
            DataExchangerThread = new Thread(new ThreadStart(dataExchanger.ThreadRun));
            System.Diagnostics.Trace.WriteLine("Before start thread");
            PhidgetThread.Name = "PhidgetThread";
            ReciverThread.Name = "ReciverThread";
            SenderThread.Name = "SenderThread";
            CalculatorThread.Name = "CalculatorThread";
            UDPReceiverThread.Name = "UDPReceiverThread";
            DataExchangerThread.Name = "DataExchangerThread";

            phidgetReciver.abort = false;
            arduinoReciver.abort = false;
            arduinoSender.abort = false;
            motionCalculator.abort = false;
            dataExchanger.abort = false;
            PhidgetThread.Start();
            ReciverThread.Start();
            CalculatorThread.Start();
            UDPReceiverThread.Start();
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
            {
                listOfKHEdits.Add(KoeffH1);
                listOfKHEdits.Add(KoeffH2);
                listOfKHEdits.Add(KoeffH3);
                listOfKHEdits.Add(KoeffH4);
                listOfKHEdits.Add(KoeffH5);
                listOfKHEdits.Add(KoeffH6);
                listOfKHEdits.Add(KoeffH7);
                listOfKHEdits.Add(KoeffH8);
                listOfKHEdits.Add(KoeffH9);
                listOfKHEdits.Add(KoeffH10);
                listOfKHEdits.Add(KoeffH11);
                listOfKHEdits.Add(KoeffH12);
            }
            {
                listOfKVEdits.Add(KoeffV1);
                listOfKVEdits.Add(KoeffV2);
                listOfKVEdits.Add(KoeffV3);
                listOfKVEdits.Add(KoeffV4);
                listOfKVEdits.Add(KoeffV5);
                listOfKVEdits.Add(KoeffV6);
                listOfKVEdits.Add(KoeffV7);
                listOfKVEdits.Add(KoeffV8);
                listOfKVEdits.Add(KoeffV9);
                listOfKVEdits.Add(KoeffV10);
                listOfKVEdits.Add(KoeffV11);
                listOfKVEdits.Add(KoeffV12);
            }

            this.Location = new Point(0, 0);

        }

        
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            phidgetReciver.Stop();
            arduinoReciver.Stop();
            arduinoSender.Stop();
            motionCalculator.Stop();
            dataExchanger.Stop();
            phidgetReciver.abort = true;
            arduinoReciver.abort = true;
            arduinoSender.abort = true;
            motionCalculator.abort = true;
            dataExchanger.abort = true;
            udpReceiver.Stop();
            abort = true;
            if(PhidgetThread.IsAlive)PhidgetThread.Join();
            if(ReciverThread.IsAlive)ReciverThread.Join();
            if(SenderThread.IsAlive)SenderThread.Join();
            if(CalculatorThread.IsAlive)CalculatorThread.Join();
            //if (UDPReceiverThread.IsAlive) UDPReceiverThread.Join();
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
            SenderThread.Start();
            phidgetReciver.Start();
            arduinoReciver.Start();
            arduinoSender.Start();
            motionCalculator.Start();
            dataExchanger.Start();
            DataExchangerThread.Start();
            StartButton.Enabled = false;
            //var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv");
            for (int i = 0; i < listOfKHEdits.Count; i++)
            {
                listOfKHEdits[i].Text = motionCalculator.getSetupAt(i).KoeffH.ToString();
                listOfKVEdits[i].Text = motionCalculator.getSetupAt(i).KoeffV.ToString();
            }
            while (!abort)
            {
                for (int i = 0; i < phidgetReciver.Count(); i++)
                {
                    listOfLEdits[i].Text = (motionCalculator.getSetupAt(i).Load- motionCalculator.getSetupAt(i).ZeroLoad).ToString("#.##");
                    listOfPEdits[i].Text = arduinoReciver.getValueOf(i).ToString("000");
                    listOfTEdits[i].Text = motionCalculator.getSetupAt(i).Direction + motionCalculator.getSetupAt(i).Torque.ToString("#.##");
                    listOfSEdits[i].Text = (motionCalculator.getSetupAt(i).KoeffHSign * motionCalculator.getSetupAt(i).CurKoeffH).ToString("#.##");
                }
                //sw.WriteLine(motionCalculator.getSetupAt(4).LastDiff + "," + phidgetReciver.getValueOf(4).ToString() + "," + motionCalculator.getSetupAt(4).Torque.ToString("#.##")+","+ motionCalculator.getSetupAt(5).LastDiff + "," + phidgetReciver.getValueOf(5).ToString() + "," + motionCalculator.getSetupAt(5).Torque.ToString("#.##"));
                //Thread.Sleep(10);
                Application.DoEvents();
            }
            //sw.Close();

        }


        private void STOPButt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void SetKoeff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listOfKHEdits.Count; i++)
            {
                motionCalculator.getSetupAt(i).KoeffH = Double.Parse(listOfKHEdits[i].Text);
                motionCalculator.getSetupAt(i).KoeffV = Double.Parse(listOfKVEdits[i].Text);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < phidgetReciver.Count(); i++)
            {
                phidgetReciver.setNoRealDevPos(i, (double)numericUpDown1.Value);
            }
        }
    }
    public class TestSetup
    {
        public int Phidget { get; set; }
        public int MotorId { get; set; }
        public double TargetLoad { get; set; }
        public double PreLoad { get; set; }
        public int MinPos { get; set; }
        public int MaxPos { get; set; }

        public int Torque { get; set; }
        public int TorqueLimit { get; set; }
        public int FriendlyMotor { get; set; }
        public double KoeffH { get; set; }
        public double KoeffV { get; set; }
        public double CurKoeffH { get; set; }
        public double CurKoeffV { get; set; }
        public double KoeffP { get; set; }
        public int KoeffVSign { get; set; }
        public int KoeffHSign { get; set; }
        public double LastLoad { get; set; }
        public double Load { get; set; }
        public double LoadLast { get; set; }
        public double ZeroLoad { get; set; }
        public int Position { get; set; }
        public double LastDiff { get; set; }
        public char Direction { get; set; }
        public int PositiveDir { get; set; }
        public string PositionRequestCommand { get; set; }
        public string MoveCommand { get; set; }
        public bool FirstPick { get; set; }
        public bool newData { get; set; }
    }
    public class TorqueAndDir
    {
        public int Torque { get; set; }
        public char Direction { get; set; }
    }
}
