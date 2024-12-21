using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomOcx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.PubSub
{
    public class Subscriber
    {
        public void GetKiwoomCandleData(string 종목코드, DateTime 기준일자)
        {
            try
            {
                주식일봉차트조회요청_Input input = new 주식일봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.기준일자 = 기준일자.ToString("yyyy:MM:dd").Replace(":", "");
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.기준일자), input.기준일자);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청)};{종목코드}";
                if (MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    )) == ERROR_CODE_DEF.NONE)
                {
                    Debug.WriteLine("send ok");
                }
                else
                {
                    Debug.WriteLine("send fail");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
