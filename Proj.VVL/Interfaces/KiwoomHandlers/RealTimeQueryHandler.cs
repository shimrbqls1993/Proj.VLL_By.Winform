using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Services.Kiwoom.Managers;

namespace Proj.VVL.Interfaces.KiwoomHandlers
{
    public class RealTimeQueryHandler : IRealTimeQueryHandler
    {
        const int MAX_REGIST_TICKER = 100;
        public static string[] Tickers = new string[MAX_REGIST_TICKER];

        public RealTimeQueryHandler()
        {
            MainForm.KiwoomOcxObj.condition.SetRealRemove();
        }

        public ERROR_CODE_DEF RegistRealData(string ticker, string screenNumber)
        {
            if (screenNumber == "0")
            {
                Debug.WriteLine("Get Screen number Failed");
                return ERROR_CODE_DEF.FAIL;
            }

            return MainForm.KiwoomOcxObj.condition.SetRealReg(screenNumber, ticker, Define.MakeFidList2String(Define.FID주식호가잔량), "0");
        }
    }
}
