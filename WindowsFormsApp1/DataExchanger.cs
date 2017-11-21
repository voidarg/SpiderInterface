using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class DataExchanger
    {
        public bool abort { get; set; }
        ArduinoReciver arduinoReciver;
        ArduinoSender arduinoSender;
        PhidgetReciver phidgetReciver;
        MotionCalculator motionCalculator;

        public DataExchanger(ref ArduinoReciver _arduinoReciver, ref ArduinoSender _arduinoSender, ref PhidgetReciver _phidgetReciver, ref MotionCalculator _motionCalculator)
        {
            arduinoReciver = _arduinoReciver;
            arduinoSender = _arduinoSender;
            phidgetReciver = _phidgetReciver;
            motionCalculator = _motionCalculator;
        }
        public void Start()
        {
        }
        public void Stop()
        {
        }
        bool initializeExchanger()
        {
            return true;
        }
        public void ThreadRun()
        {
            initializeExchanger();
            System.Diagnostics.Trace.WriteLine("DataExchanger started");

            while (!abort)
            {
                while (!abort)
                {
                    for (int i = 0; i < phidgetReciver.Count(); i++)
                    {
                        motionCalculator.getSetupAt(i).Load = phidgetReciver.getValueOf(i);
                        motionCalculator.getSetupAt(i).Position = arduinoReciver.getValueOf(i);
                        arduinoSender.setValueOf(i, motionCalculator.getSetupAt(i).Torque, motionCalculator.getSetupAt(i).Direction);
                    }
                }
            }

            System.Diagnostics.Trace.WriteLine("DataExchanger stopped");
        }
    }
}
