using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomOcx;
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
        static Mutex mut = new Mutex();
        System.Threading.Timer timer_ClearTrCount;
        AutoResetEvent autoResetEvent;
        const int MAX_TR_CNT = 5;
        public static int TR_CNT = 0;

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

        /// <summary>
        /// 조회시 1초당 5회 횟수는 CommRqData(), CommKwRqData(), SendCondition() 함수 사용이 합산되고
        /// 조회와 별개로 주문시 1초당 5회 횟수는 SendOrder(), SendOrderFO() 함수 사용이 합산됩니다.
        /// </summary>
        public ERROR_CODE_DEF Send(ERROR_CODE_DEF func)
        {
            ERROR_CODE_DEF result = ERROR_CODE_DEF.FULL_TR_SEND;

            mut.WaitOne();
            if (TR_CNT < MAX_TR_CNT)
            {
                result = func;
            }
            mut.ReleaseMutex();

            return result;
        }
    }
}
