using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class SendTrManager : IServiceManager
    {
        System.Threading.Timer timer_ClearTrCount;
        AutoResetEvent autoResetEvent;
        public Model.UiOption.UI_STATUS Status { get; set; }
        
        public void Start()
        {
            autoResetEvent = new AutoResetEvent(false);
            timer_ClearTrCount = new System.Threading.Timer(ClearTrCount, autoResetEvent, 0, 1000);
        }

        public void Stop()
        {
            autoResetEvent.WaitOne();
            timer_ClearTrCount.Dispose();
        }

        void ClearTrCount(object? sender)
        {
            SendTrHandler.TR_CNT = 0;
        }
    }
}
