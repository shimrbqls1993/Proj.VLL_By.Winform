using Microsoft.Extensions.DependencyInjection;
using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class KiwoomServiceManager
    {
        public LoginManager loginManager;
        public SendTrManager sendTrManager;
        public PublishTickerManager recommandTickerManager;
        public KiwoomServiceManager(IRecommandTickerHandler hRecommandTicker)
        {
            loginManager = new LoginManager();
            sendTrManager = new SendTrManager();
            recommandTickerManager = new PublishTickerManager(hRecommandTicker);
        }

        public void ServiceStart()
        {
            loginManager.Start();
            sendTrManager.Start();
        }

        public void ServiceStop()
        {
            loginManager?.Stop();
            sendTrManager?.Stop();
        }
    }
}
