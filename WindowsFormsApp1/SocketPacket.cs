using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public struct Leg
    {
        public int front;
        public int back;
        public int under;
        public int over;
    }
    public class SocketPacket
        {
        public SocketPacket(string username, decimal sum, int period)
        {
            //this.Id = int.Parse(Guid.NewGuid().ToString()); // генерируем номер 
        }
        //public int Id { get; private set; } // id - номер счета

        public Leg LLeg { get; private set; }
        public Leg RLeg { get; private set; }
        public Leg LHand { get; private set; }
        public Leg RHand { get; private set; }
    }
}