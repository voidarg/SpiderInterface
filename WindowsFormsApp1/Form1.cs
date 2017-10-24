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
                MtrToPh.Add( 2,10);
                MtrToPh.Add( 3,11);
                MtrToPh.Add( 0,4);
                MtrToPh.Add( 1,5);
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
            arduinoSender = new ArduinoSender(ref listofPorts, ref PhToMtr);
            arduinoReciver = new ArduinoReciver(ref listofPorts,ref MtrToPh);
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
            //var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv");

            while (!abort)
            {
                for (int i = 0; i < phidgetReciver.Count(); i++)
                {
                    listOfLEdits[i].Text = (motionCalculator.getSetupAt(i).Load - motionCalculator.getSetupAt(i).ZeroLoad).ToString("#.##");
                    listOfPEdits[i].Text = arduinoReciver.getValueOf(i).ToString("#.##");
                    listOfTEdits[i].Text = motionCalculator.getSetupAt(i).Direction + motionCalculator.getSetupAt(i).Torque.ToString("#.##");
                    listOfSEdits[i].Text = motionCalculator.getSetupAt(i).LastDiff.ToString("#.##");
                    motionCalculator.getSetupAt(i).Load = phidgetReciver.getValueOf(i);
                    motionCalculator.getSetupAt(i).Position = arduinoReciver.getValueOf(i);
                    arduinoSender.setValueOf(i, motionCalculator.getSetupAt(i).Torque, motionCalculator.getSetupAt(i).Direction);
                }
                //sw.WriteLine(motionCalculator.getSetupAt(4).LastDiff + "," + phidgetReciver.getValueOf(4).ToString() + "," + motionCalculator.getSetupAt(4).Torque.ToString("#.##")+","+ motionCalculator.getSetupAt(5).LastDiff + "," + phidgetReciver.getValueOf(5).ToString() + "," + motionCalculator.getSetupAt(5).Torque.ToString("#.##"));
                //Thread.Sleep(10);
                Application.DoEvents();
            }
            //sw.Close();

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
        public int PositiveDir { get; set; }
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
