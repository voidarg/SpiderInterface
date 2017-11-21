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
                testSetup.PreLoad = 0;
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
                testSetup.KoeffH = 1;
                testSetup.KoeffV = 0.1;
                testSetup.CurKoeffH = testSetup.KoeffH;
                testSetup.CurKoeffV = testSetup.KoeffV;
                testSetup.KoeffP = 1;
                testSetup.KoeffVSign = 1;
                testSetup.KoeffHSign = 1;
                testSetup.Direction = 'F';
                testSetup.PositiveDir= 1;
                testSetup.FriendlyMotor = i;
                listOfSetups.Add(testSetup);
            }
            {
                listOfSetups[4].FriendlyMotor = 5;
                listOfSetups[5].FriendlyMotor = 4;
                listOfSetups[10].FriendlyMotor = 11;
                listOfSetups[11].FriendlyMotor = 10;
                listOfSetups[4].PreLoad = 0;
                listOfSetups[5].PreLoad = 0;
                listOfSetups[10].PreLoad = 0;
                listOfSetups[11].PreLoad = 0;
                listOfSetups[10].MinPos = 300;
                listOfSetups[10].MaxPos = 700;
                listOfSetups[11].MinPos = 300;
                listOfSetups[11].MaxPos = 700;
            }
            return true;
        }
        public void ThreadRun()
        {
            System.IO.File.Delete("c:\\tmp\\runlog.csv");
            var sw = System.IO.File.AppendText("c:\\tmp\\runlog.csv");
            initializeCalculator();
            System.Diagnostics.Trace.WriteLine("MotionCalculator started");
            Thread.Sleep(2500);
            while (listOfSetups[0].Load == 0 || listOfSetups[listOfSetups.Count - 1].Load == 0)
            {
                if (abort)
                {
                    return;
                };
            }
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
            sw.WriteLine("Start of log ZeroLoad Front:" + listOfSetups[10].ZeroLoad.ToString() + "," + "ZeroLoad BAck:" + listOfSetups[11].ZeroLoad.ToString() + "," + "KoeffH:" + listOfSetups[5].KoeffH.ToString() + "," + "KoeffV:" + listOfSetups[5].KoeffV.ToString());
            sw.WriteLine("L1,L2,MF,MB,T1,CurKoeffV,T2,CurKoeffH");
            while (!abort)
            {
                for (int i = 0; i < listOfSetups.Count; i++)
                {
                    // request position
                    if (listOfSetups[i].LastLoad == listOfSetups[i].Load)
                    {
                        continue;
                    }
                    listOfSetups[i].LastLoad = listOfSetups[i].Load;

                    int FriendID = listOfSetups[i].FriendlyMotor;
                    double L2;
                    double L2z;
                    double centeringF;
                    double centeringB;

                    if (FriendID > i) continue;
                    if (FriendID == i)
                    {
                        L2 = 0;
                        L2z = 0;
                    }
                    else
                    {
                        L2 = listOfSetups[FriendID].Load;
                        L2z = listOfSetups[FriendID].ZeroLoad;
                    }
                    centeringF = (listOfSetups[FriendID].Position - 530) * 1/1000;
                    centeringB = (listOfSetups[i].Position - 530) * 1 /1000;
                    double L1 = listOfSetups[i].Load;
                    double L1z = listOfSetups[i].ZeroLoad;
                    double T1 = ((((L2 - L2z) + (L1 - L1z)) + listOfSetups[i].PreLoad) * listOfSetups[i].CurKoeffV); //UpDown

                    if (Math.Sign(listOfSetups[i].KoeffVSign) == Math.Sign(T1))
                    {
                        listOfSetups[i].CurKoeffV *= 1.01;
                    }
                    else
                    {
                        listOfSetups[i].CurKoeffV = listOfSetups[i].KoeffV;
                    }
                    listOfSetups[i].KoeffVSign = Math.Sign(T1);

                    if (listOfSetups[i].CurKoeffV > 20)
                    {
                        listOfSetups[i].CurKoeffV = 20;
                    }
                    else if (listOfSetups[i].CurKoeffV < -20)
                    {
                        listOfSetups[i].CurKoeffV = -20;
                    }

                    double T2 = (((L2 - L2z) - (L1 - L1z))                             * listOfSetups[i].KoeffH) ; //LeftRight


                    if (Math.Sign(listOfSetups[i].KoeffHSign) == Math.Sign(T2))
                    {
                        listOfSetups[i].CurKoeffH *= 1.1;
                    }
                    else
                    {
                        listOfSetups[i].CurKoeffH = listOfSetups[i].KoeffH;
                    }
                    listOfSetups[i].KoeffHSign = Math.Sign(T2);

                    if (listOfSetups[i].CurKoeffH > 20)
                    {
                        listOfSetups[i].CurKoeffH = 20;
                    }
                    else if (listOfSetups[i].CurKoeffH < -20)
                    {
                        listOfSetups[i].CurKoeffH = -20;
                    }

                    double M1 = T1;
                    double M2 = T1;
                    double M3 = T2;
                    double M4 = T2*(-1);

                    double MF = M1 + M3 ;
                    double MB = M2 + M4 ;


                    if ((int)MB > listOfSetups[i].TorqueLimit)
                    {
                        listOfSetups[i].Torque = listOfSetups[i].TorqueLimit;
                    }
                    else if ((int)MB < (-1 * listOfSetups[i].TorqueLimit))
                    {
                        listOfSetups[i].Torque = (-1 * listOfSetups[i].TorqueLimit);
                    }
                    else
                    {
                        listOfSetups[i].Torque = (int)MB;
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
                    if (FriendID != i)
                    {
                        if ((int)MF > listOfSetups[FriendID].TorqueLimit)
                        {
                            listOfSetups[FriendID].Torque = listOfSetups[FriendID].TorqueLimit;
                        }
                        else if ((int)MF < (-1 * listOfSetups[FriendID].TorqueLimit))
                        {
                            listOfSetups[FriendID].Torque = (-1 * listOfSetups[FriendID].TorqueLimit);
                        }
                        else
                        {
                            listOfSetups[FriendID].Torque = (int)MF;
                        }
                        if (listOfSetups[FriendID].Torque * listOfSetups[FriendID].PositiveDir > 0)   // go back
                        {
                            listOfSetups[FriendID].Direction = 'B';
                        }
                        else if (listOfSetups[FriendID].Torque * listOfSetups[FriendID].PositiveDir < 0) // go forward
                        {
                            listOfSetups[FriendID].Direction = 'F';
                        }
                        else
                        {
                            listOfSetups[FriendID].Direction = 'F';
                        }
                    }
                    if (i==11)
                    {
                        sw.WriteLine(L1+","+L2 + "," + MF + "," + MB + "," + T1 + "," + listOfSetups[i].CurKoeffV + "," + T2 + "," + listOfSetups[i].CurKoeffH + ",");
                    }
                }
            }
            sw.Close();
            System.Diagnostics.Trace.WriteLine("MotionCalculator stopped");
        }
    }
}
