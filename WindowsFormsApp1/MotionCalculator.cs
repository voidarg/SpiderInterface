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
                testSetup.TorqueLimit = 250;
                testSetup.Koeff = 0.02;
                testSetup.Direction = 'S';
                testSetup.PositiveDir= 1;
                listOfSetups.Add(testSetup);
            }
            {
                //listOfSetups[4].PositiveDir = -1;
                //listOfSetups[5].PositiveDir = -1;
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
            sw.Write("CurDiff1,CurDiff1-CurDiff2,DiffSums,CurLoad,Torque,");
            sw.WriteLine("CurDiff1,CurDiff1-CurDiff2,DiffSums,CurLoad,Torque,");
            while (!abort)
            {
                //System.Diagnostics.Trace.WriteLine("MotionCalculator is ok");
                Thread.Sleep(10);
                for (int i = 4; i < 6/*listOfSetups.Count*/; i++)
                {
                    // request position

                    double diff = 0;
                    double diff2 = 0; 
                    if ( i == 4)
                    {
                         diff = (listOfSetups[i].TargetLoad + listOfSetups[i].Load - listOfSetups[i].ZeroLoad);
                         diff2 = (listOfSetups[i+1].TargetLoad + listOfSetups[i +1].Load - listOfSetups[i + 1].ZeroLoad);
                    }
                    if (i == 5)
                    {
                         diff = (listOfSetups[i].TargetLoad + listOfSetups[i].Load - listOfSetups[i].ZeroLoad);
                         diff2 = (listOfSetups[i - 1].TargetLoad + listOfSetups[i - 1].Load - listOfSetups[i - 1].ZeroLoad);
                    }
                    if (Math.Sign(diff) == Math.Sign(diff2))
                    {
                        diff2 = 0;
                    }
                    //if (Math.Abs(diff) < 50) diff = 0;
                    listOfSetups[i].LastDiff += Math.Pow(diff-diff2, 1);
                    //if (listOfSetups[i].LastDiff > 0 && diff < 0) listOfSetups[i].LastDiff *=0.5;
                    //if (listOfSetups[i].LastDiff < 0 && diff > 0) listOfSetups[i].LastDiff *=0.5;
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
                        listOfSetups[i].Torque = -40; listOfSetups[i].LastDiff = 0;
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
                    if ( i  == 4)
                    {
                        sw.Write(diff + ","+ (diff - diff2) + "," + listOfSetups[i].LastDiff + "," + listOfSetups[i].Load.ToString() + "," + listOfSetups[i].Torque.ToString("#.##") + ",");
                    }
                    if (i == 5)
                    {
                        sw.WriteLine(diff + "," + (diff - diff2) + "," + listOfSetups[i].LastDiff + "," + listOfSetups[i].Load.ToString() + "," + listOfSetups[i].Torque.ToString("#.##") + ",");
                    }
                }
            }
            sw.Close();
            System.Diagnostics.Trace.WriteLine("MotionCalculator stopped");
        }
    }
}
