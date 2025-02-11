using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.PubSub;
using Proj.VVL.Services.Abstractions;
using Proj.VVL.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Service.Common.Manager
{
    public class CommonServiceManager
    {
//        public RecommandTickerHandler RecommandTicker = new RecommandTickerHandler();
//        public Publisher PublishTicker;
        public LocalHttpServer LocalGuiComm;

        public CommonServiceManager() 
        {
            LocalGuiComm = new LocalHttpServer();
        }

        public void Start()
        {
            LocalGuiComm.Start();
        }

        public void Stop()
        {
            LocalGuiComm?.Stop();
        }
    }
}
