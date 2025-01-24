using AxKHOpenAPILib;
using LiveChartsCore.Defaults;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.DataInventoryHandlers;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    internal class ReceiveDataHandlerDef
    {
        public AxKHOpenAPI OcxObject;

        public ReceiveDataHandlerDef(AxKHOpenAPI OcxObjBind) 
        {
            OcxObject = OcxObjBind;
            OcxObject.OnReceiveTrData += RegistOnReceiveTrData;
            OcxObject.OnReceiveRealData += RegistOnReceiveRealData;
        }

        /// <summary>
        /// OnReceiveTRData()이벤트가 발생될때 수신한 데이터를 얻어오는 함수입니다.
        /// 이 함수는 OnReceiveTRData()이벤트가 발생될때 그 안에서 사용해야 합니다.
        /// </summary>
        /// <param name="TR이름"></param>
        /// <param name="레코드이름"></param>
        /// <param name="nIndex번째"></param>
        /// <param name="출력항목이름"></param>
        /// <returns></returns>
        public string GetCommData(string TR이름, string 레코드이름, int nIndex번째, string 출력항목이름)
        {
            return OcxObject.GetCommData(TR이름, 레코드이름, nIndex번째, 출력항목이름);
        }

        /// <summary>
        /// 조회 수신데이터 크기가 큰 차트데이터를 한번에 가져올 목적으로 만든 차트조회 전용함수입니다.
        /// </summary>
        /// <param name="TR이름"></param>
        /// <param name="레코드이름"></param>
        /// <returns></returns>
        public object GetCommDataEx(string TR이름, string 레코드이름)
        {
            return OcxObject.GetCommDataEx(TR이름, 레코드이름);
        }

        /// <summary>
        /// Send시 RQName의 포맷은 $"{nameof(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청)};{종목코드};{(int)CANDLE_TIME_FRAME_DEF}";
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RegistOnReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string[] parseRqName = e.sRQName.Split(';');
            string TrCode = parseRqName[0];
            string Ticker = parseRqName[1];
            CANDLE_TIME_FRAME_DEF TimeFrame = (CANDLE_TIME_FRAME_DEF)int.Parse(parseRqName[2]);
            switch (TrCode)
            {
                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식주봉차트조회요청):
                    OPT주식주봉차트조회요청(e, Ticker, TimeFrame);
                    break;
                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청):
                    OPT주식일봉차트조회요청(e, Ticker, TimeFrame);
                    break;
                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청):
                    OPT주식분봉차트조회요청(e, Ticker, TimeFrame);
                    break;
            }
        }

        /// <summary>
        /// Datetime string format = YYYYMMDD
        /// </summary>
        /// <param name="eventHandle"></param>
        /// <param name="code"></param>
        /// <param name="timeFrame"></param>
        public void OPT주식일봉차트조회요청(_DKHOpenAPIEvents_OnReceiveTrDataEvent eventHandle, string code, CANDLE_TIME_FRAME_DEF timeFrame)
        {
            object[,]recvDatas = (object[,])GetCommDataEx(eventHandle.sTrCode,eventHandle.sRQName);
            List<CANDLE_STICK_DEF> datas = new List<CANDLE_STICK_DEF>();
            DateTime timeTmp;
            for (int row = recvDatas.GetLength(0) - 1; row >= 0; row--)
            {
                CANDLE_STICK_DEF candleData = new CANDLE_STICK_DEF();
                candleData.Close = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.현재가]);
                candleData.Volume = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.거래량]);
                candleData.Open = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.시가]);
                candleData.High = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.고가]);
                candleData.Low = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.저가]);
                candleData.Datetime = (string)recvDatas[row, (int)주식일봉차트조회요청_Output.일자];
                datas.Add(candleData);
            }
            CandleSticks storeData = new CandleSticks(code);
            storeData.AddChartCandleStick(datas.ToArray(), timeFrame);
        }

        /// <summary>
        /// Datetime string format = YYYYMMDDHHmm
        /// </summary>
        /// <param name="eventHandle"></param>
        /// <param name="code"></param>
        /// <param name="timeFrame"></param>
        public void OPT주식분봉차트조회요청(_DKHOpenAPIEvents_OnReceiveTrDataEvent eventHandle, string code, CANDLE_TIME_FRAME_DEF timeFrame)
        {
            object[,] recvDatas = (object[,])GetCommDataEx(eventHandle.sTrCode, eventHandle.sRQName);
            List<CANDLE_STICK_DEF> datas = new List<CANDLE_STICK_DEF>();
            DateTime timeTmp;
            for (int row = 0; row < recvDatas.GetLength(0); row++)
            {
                CANDLE_STICK_DEF candleData = new CANDLE_STICK_DEF();
                candleData.Close = int.Parse((string)recvDatas[row, (int)주식분봉차트조회요청_Output.현재가]);
                candleData.Volume = int.Parse((string)recvDatas[row, (int)주식분봉차트조회요청_Output.거래량]);
                candleData.Open = int.Parse((string)recvDatas[row, (int)주식분봉차트조회요청_Output.시가]);
                candleData.High = int.Parse((string)recvDatas[row, (int)주식분봉차트조회요청_Output.고가]);
                candleData.Low = int.Parse((string)recvDatas[row, (int)주식분봉차트조회요청_Output.저가]);
                candleData.Datetime = (string)recvDatas[row, (int)주식분봉차트조회요청_Output.체결시간];
                datas.Add(candleData);
            }
            CandleSticks storeData = new CandleSticks(code);
            storeData.AddChartCandleStick(datas.ToArray(), timeFrame);
        }

        /// <summary>
        /// Time String Format = YYYYMMDD
        /// </summary>
        /// <param name="eventHandle"></param>
        /// <param name="code"></param>
        /// <param name="timeFrame"></param>
        public void OPT주식주봉차트조회요청(_DKHOpenAPIEvents_OnReceiveTrDataEvent eventHandle, string code, CANDLE_TIME_FRAME_DEF timeFrame)
        {
            object[,] recvDatas = (object[,])GetCommDataEx(eventHandle.sTrCode, eventHandle.sRQName);
            List<CANDLE_STICK_DEF> datas = new List<CANDLE_STICK_DEF>();
            for (int row = 0; row < recvDatas.GetLength(0); row++)
            {
                CANDLE_STICK_DEF candleData = new CANDLE_STICK_DEF();
                candleData.Close = int.Parse((string)recvDatas[row, (int)주식주봉차트조회요청_Output.현재가]);
                candleData.Volume = int.Parse((string)recvDatas[row, (int)주식주봉차트조회요청_Output.거래량]);
                candleData.Open = int.Parse((string)recvDatas[row, (int)주식주봉차트조회요청_Output.시가]);
                candleData.High = int.Parse((string)recvDatas[row, (int)주식주봉차트조회요청_Output.고가]);
                candleData.Low = int.Parse((string)recvDatas[row, (int)주식주봉차트조회요청_Output.저가]);
                candleData.Datetime = (string)recvDatas[row, (int)주식주봉차트조회요청_Output.일자];
                datas.Add(candleData);
            }
            CandleSticks storeData = new CandleSticks(code);
            storeData.AddChartCandleStick(datas.ToArray(), timeFrame);
        }

        /// <summary>
        /// 실시간시세 데이터가 수신될때마다 종목단위로 발생됩니다.
        /// SetRealReg()함수로 등록한 실시간 데이터도 이 이벤트로 전달됩니다.
        /// GetCommRealData()함수를 이용해서 수신된 데이터를 얻을수 있습니다.
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void RegistOnReceiveRealData(object sender, _DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            Debug.WriteLine(e.sRealData);
            Debug.WriteLine(e.sRealKey);
            Debug.WriteLine(e.sRealType);
            /*
            string result = string.Empty;
            foreach(실시간FID fid in Define.FID주식호가잔량)
            {
                result = OcxObject.GetCommRealData(e.sRealKey, (int)fid);
                Debug.WriteLine($"{fid} : {result}");
            }
            */
        }
    }
}
