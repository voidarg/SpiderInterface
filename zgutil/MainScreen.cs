using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zgutil
{
    class MainScreen : IScreen
    {
        public event OnFinishedProcessingCallback OnFinishedProcessing;

        public void Draw()
        {
            
        }

        public void ProcessCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
