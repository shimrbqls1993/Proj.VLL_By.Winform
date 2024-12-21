using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class ScreenNumberManager : IServiceManager
    {
        public IScreenNumberHandler Handler;
        public ScreenNumberManager(IScreenNumberHandler _handler) 
        {
            Handler = _handler;
        }

        public void Start()
        {

        }

        public void Stop() 
        {

        }
    }
}
