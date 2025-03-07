using Proj.VVL.Interfaces.DataInventoryHandlers;
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
    /// <summary>
    /// kiwoom subscriber api
    /// </summary>

    public class Subscriber
    {
        public ERROR_CODE_DEF GetKiwoomCandleData(string 종목코드, CANDLE_TIME_FRAME_DEF timeFrame)
        {
            ERROR_CODE_DEF result = ERROR_CODE_DEF.NONE;
            switch (timeFrame)
            {
                case CANDLE_TIME_FRAME_DEF.DAY:
                    return GetKiwoomCandleData_일봉(종목코드);
                case CANDLE_TIME_FRAME_DEF.MIN_1:
                    return GetKiwoomCandleData_1분봉(종목코드);
                case CANDLE_TIME_FRAME_DEF.MIN_5:
                    return GetKiwoomCandleData_5분봉(종목코드);
                case CANDLE_TIME_FRAME_DEF.HOUR:
                    return GetKiwoomCandleData_1시간봉(종목코드);
                case CANDLE_TIME_FRAME_DEF.WEEK:
                    return GetKiwoomCandleData_주봉(종목코드);
            }
            return ERROR_CODE_DEF.IS_NOT_VALID_COMMAND;
        }

        private ERROR_CODE_DEF GetKiwoomCandleData_일봉(string 종목코드)
        {
            try
            {
                주식일봉차트조회요청_Input input = new 주식일봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.기준일자 = DateTime.Now.ToString("yyyy:MM:dd").Replace(":", "");
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.기준일자), input.기준일자);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF.DAY}";
                
                return MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    ));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return ERROR_CODE_DEF.FAIL;
            }
        }

        private ERROR_CODE_DEF GetKiwoomCandleData_주봉(string 종목코드)
        {
            try
            {
                주식주봉차트조회요청_Input input = new 주식주봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.기준일자 = DateTime.Now.ToString("yyyy:MM:dd").Replace(":", "");
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.기준일자), input.기준일자);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식주봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF.WEEK}";
                return MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식주봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    ));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return ERROR_CODE_DEF.FAIL;
            }
        }

        private ERROR_CODE_DEF GetKiwoomCandleData_1분봉(string 종목코드)
        {
            try
            {
                주식분봉차트조회요청_Input input = new 주식분봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.틱범위 = "1";
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.틱범위), input.틱범위);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF.MIN_1}";
                return MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    ));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return ERROR_CODE_DEF.FAIL;
            }
        }

        private ERROR_CODE_DEF GetKiwoomCandleData_5분봉(string 종목코드)
        {
            try
            {
                주식분봉차트조회요청_Input input = new 주식분봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.틱범위 = "5";
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.틱범위), input.틱범위);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF.MIN_5}";
                return MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    ));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return ERROR_CODE_DEF.FAIL;
            }
        }

        private ERROR_CODE_DEF GetKiwoomCandleData_1시간봉(string 종목코드)
        {
            try
            {
                주식분봉차트조회요청_Input input = new 주식분봉차트조회요청_Input();
                input.종목코드 = 종목코드;
                input.틱범위 = "60";
                input.수정주가구분 = "0";
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.종목코드), input.종목코드);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.틱범위), input.틱범위);
                MainForm.KiwoomOcxObj.query.SetInputValue(nameof(input.수정주가구분), input.수정주가구분);
                string RQName = $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF.HOUR}";
                return MainForm.Instance.KiwoomServices.sendTrManager.Send(
                    MainForm.KiwoomOcxObj.query.CommRqData(RQName,
                    Define.CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청),
                    0,
                    MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString()
                    ));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return ERROR_CODE_DEF.FAIL;
            }
        }
    
        public void GetRealTimeData(string 종목코드)
        {
            RealTimeQueryHandler handle = new RealTimeQueryHandler();

            handle.RegistRealData(종목코드, MainForm.Instance.KiwoomServices.screenNumberManager.Handler.GetScreenNumber().ToString());
        }
    }
}
