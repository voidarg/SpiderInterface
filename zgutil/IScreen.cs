using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zgutil
{
    public delegate void OnFinishedProcessingCallback (IScreen me);
    public interface IScreen
    {
        void Draw();
        void ProcessCommand(String command);
        event OnFinishedProcessingCallback OnFinishedProcessing;
    }
}
