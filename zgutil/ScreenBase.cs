using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zgutil
{
    public class ScreenBase : IScreen
    {
        public event OnFinishedProcessingCallback OnFinishedProcessing;

        abstract public void ProcessCommand(string command);

        protected void Draw ()
        {

        }

    }
}
