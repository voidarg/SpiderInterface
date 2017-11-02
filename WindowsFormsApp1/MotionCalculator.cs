using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsApp1
{
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
                testSetup.TorqueLimit = 255;
                testSetup.Koeff = 1;
                testSetup.Direction = 'F';
                testSetup.PositiveDir= 1;
                listOfSetups.Add(testSetup);
            }
            {
            }
            return true;
        }
        public void ThreadRun()
        {
            var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv");
            initializeCalculator();
            System.Diagnostics.Trace.WriteLine("MotionCalculator started");
            Thread.Sleep(2500);
            while (listOfSetups[0].Load == 0 || listOfSetups[listOfSetups.Count - 1].Load == 0) ;
            for (int j = 0; j < 1000; j++)
            {
                for (int i = 0; i < listOfSetups.Count; i++)
                {
                    listOfSetups[i].ZeroLoad += listOfSetups[i].Load;
                }
            }
            for (int i = 0; i < listOfSetups.Count; i++)
            {
                listOfSetups[i].ZeroLoad = listOfSetups[i].ZeroLoad / 1000;
            }
            sw.WriteLine("Start of log ZeroLoad Front:" + listOfSetups[4].ZeroLoad.ToString() + "," + "ZeroLoad BAck:" + listOfSetups[5].ZeroLoad.ToString() + "," + "Koeff:" + listOfSetups[5].Koeff.ToString());
            //sw.Write("CurDiff1,DiffSums,CurLoad,Torque,");
            sw.WriteLine("L1,L2,MF,MB,");
            while (!abort)
            {
                //Thread.Sleep(10);
                for (int i = 5; i < 6/*listOfSetups.Count*/; i++)
                {
                    // request position


                    double L1 = listOfSetups[i].Load;
                    double L2 = listOfSetups[i - 1].Load;
                    double L1z = listOfSetups[i].ZeroLoad;
                    double L2z = listOfSetups[i - 1].ZeroLoad;
                    double T1 = ((((L2 - L2z) + (L1 - L1z)) + 400) / 10) * (1 + Math.Abs((L2 - L2z) + (L1 - L1z)) / 1000);         //UpDown
                    double T2 = (((L2 - L2z) - (L1 - L1z)) / 1) * (1 + Math.Abs((L2 - L2z) - (L1 - L1z)) / 50);    //LeftRight
                    double M1 = T1;
                    double M2 = T1;
                    double M3 = T2;
                    double M4 = T2 * (-1);

                    double MF = M1 + M3;
                    double MB = M2 + M4;

                    listOfSetups[i - 1].Torque = (int)MF;
                    listOfSetups[i].Torque = (int)MB;

                    if (listOfSetups[i].Torque > listOfSetups[i].TorqueLimit)
                    {
                        listOfSetups[i].Torque = listOfSetups[i].TorqueLimit;
                    }
                    else if (listOfSetups[i].Torque < (-1 * listOfSetups[i].TorqueLimit))
                    {
                        listOfSetups[i].Torque = (-1 * listOfSetups[i].TorqueLimit);
                    }
                    if (listOfSetups[i].Torque * listOfSetups[i].PositiveDir > 0)   // go back
                    {
                        listOfSetups[i].Direction = 'B';
                    }
                    else if (listOfSetups[i].Torque * listOfSetups[i].PositiveDir < 0) // go forward
                    {
                        listOfSetups[i].Direction = 'F';
                    }
                    else
                    {
                        listOfSetups[i].Direction = 'F';
                    }

                    if (listOfSetups[i - 1].Torque > listOfSetups[i - 1].TorqueLimit)
                    {
                        listOfSetups[i - 1].Torque = listOfSetups[i - 1].TorqueLimit;
                    }
                    else if (listOfSetups[i - 1].Torque < (-1 * listOfSetups[i - 1].TorqueLimit))
                    {
                        listOfSetups[i - 1].Torque = (-1 * listOfSetups[i - 1].TorqueLimit);
                    }
                    if (listOfSetups[i - 1].Torque * listOfSetups[i - 1].PositiveDir > 0)   // go back
                    {
                        listOfSetups[i - 1].Direction = 'B';
                    }
                    else if (listOfSetups[i - 1].Torque * listOfSetups[i - 1].PositiveDir < 0) // go forward
                    {
                        listOfSetups[i - 1].Direction = 'F';
                    }
                    else
                    {
                        listOfSetups[i - 1].Direction = 'F';
                    }
                    sw.Write(L1 + "," + L2 + "," + MF + "," + MB + ",");
                }
                //System.Diagnostics.Trace.WriteLine("MotionCalculator is ok");
                Thread.Sleep(10);
                for (int i = 11; i < 12/*listOfSetups.Count*/; i++)
                {
                    // request position

                    double L1 = listOfSetups[i].Load;
                    double L2 = listOfSetups[i-1].Load;
                    double L1z = listOfSetups[i].ZeroLoad;
                    double L2z = listOfSetups[i - 1].ZeroLoad;
                    double T1 = (((L2 - L2z)+(L1 - L1z))+500) / 10;
                    double T2 = ((L2-L2z) - (L1-L1z))/ 0.5;
                    double M1 = T1;
                    double M2 = T1;
                    double M3 = T2;
                    double M4 = T2*(-1);

                    double MF = M1 + M3;
                    double MB = M2 + M4;

                    listOfSetups[i - 1].Torque = (int)MF;
                    listOfSetups[i].Torque = (int)MB;

                    if (listOfSetups[i].Torque > listOfSetups[i].TorqueLimit)
                    {
                        listOfSetups[i].Torque = listOfSetups[i].TorqueLimit;
                    }
                    else if (listOfSetups[i].Torque < (-1 * listOfSetups[i].TorqueLimit))
                    {
                        listOfSetups[i].Torque = (-1 * listOfSetups[i].TorqueLimit);
                    }
                    if (listOfSetups[i].Torque * listOfSetups[i].PositiveDir > 0)   // go back
                    {
                        listOfSetups[i].Direction = 'B';
                    }
                    else if (listOfSetups[i].Torque * listOfSetups[i].PositiveDir < 0) // go forward
                    {
                        listOfSetups[i].Direction = 'F';
                    }
                    else
                    {
                        listOfSetups[i].Direction = 'F';
                    }

                    if (listOfSetups[i-1].Torque > listOfSetups[i-1].TorqueLimit)
                    {
                        listOfSetups[i-1].Torque = listOfSetups[i-1].TorqueLimit;
                    }
                    else if (listOfSetups[i-1].Torque < (-1 * listOfSetups[i-1].TorqueLimit))
                    {
                        listOfSetups[i-1].Torque = (-1 * listOfSetups[i-1].TorqueLimit);
                    }
                    if (listOfSetups[i-1].Torque * listOfSetups[i-1].PositiveDir > 0)   // go back
                    {
                        listOfSetups[i-1].Direction = 'B';
                    }
                    else if (listOfSetups[i-1].Torque * listOfSetups[i-1].PositiveDir < 0) // go forward
                    {
                        listOfSetups[i-1].Direction = 'F';
                    }
                    else
                    {
                        listOfSetups[i-1].Direction = 'F';
                    }
                    sw.WriteLine(L1 + "," + L2 + "," + MF + "," + MB + ",");
                }
                

            }
            sw.Close();
            System.Diagnostics.Trace.WriteLine("MotionCalculator stopped");
        }
    }
}
