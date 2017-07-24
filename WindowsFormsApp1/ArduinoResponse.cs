using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    // {M0,P411}<0>
    class ArduinoResponse
    {
        private int step;

        public enum ResponseType
        {
            Status,
            ResultCode
        }

        public bool Read (String response)
        {


            foreach (char ch in response)
            {
                switch (ch)
                {
                    case '{':
                        break;
                    case '}':
                        break;
                    case '<':
                        break;
                    case '>':
                        break;
                    case 'M':
                        break;
                    case 'P':
                        break;
                    default:
                        {
                            break;
                        }
                }
            }
            return true;
        }
    }
}
