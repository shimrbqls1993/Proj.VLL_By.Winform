using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Interfaces.KiwoomOcx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Proj.VVL.Interfaces.KiwoomHandlers
{
    public class SendTrHandler : ISendTrHandler
    {
        static Mutex mut = new Mutex();
        System.Threading.Timer timer_ClearTrCount;
        AutoResetEvent autoResetEvent;
        const int MAX_TR_CNT = 5;
        public static int TR_CNT = 0;

        /// <summary>
        /// 조회시 1초당 5회 횟수는 CommRqData(), CommKwRqData(), SendCondition() 함수 사용이 합산되고
        /// 조회와 별개로 주문시 1초당 5회 횟수는 SendOrder(), SendOrderFO() 함수 사용이 합산됩니다.
        /// </summary>
        public SendTrHandler()
        {
            autoResetEvent = new AutoResetEvent(false);
            timer_ClearTrCount = new System.Threading.Timer(ClearTrCount, autoResetEvent, 0, 1000);
        }

        void ClearTrCount(object? sender)
        {
            TR_CNT = 0;
        }

        public ERROR_CODE_DEF Send(Func<ERROR_CODE_DEF> func)
        {
            ERROR_CODE_DEF result = ERROR_CODE_DEF.FULL_TR_SEND;

            mut.WaitOne();
            if (TR_CNT < MAX_TR_CNT)
            {
                result = func();
            }
            mut.ReleaseMutex();

            return result;
        }

        /*
        public ERROR_CODE_DEF ReqTransaction(KIWOOM_OPT_TR_CODE_DEF optCode)
        {
            switch (optCode) 
            {
                case KIWOOM_OPT_TR_CODE_DEF.주식기본정보요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식거래원요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.체결정보요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식호가요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식일주월시분요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식시분요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.미체결요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.체결요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식틱차트조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.주식주봉차트조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.업종틱차트조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.업종분봉조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.업종일봉조회요청:
                    break;
                case KIWOOM_OPT_TR_CODE_DEF.업종주봉조회요청:
                    break;
            }
        }
        */

    }
}
