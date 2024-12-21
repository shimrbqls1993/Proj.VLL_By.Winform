using AxKHOpenAPILib;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.DataInventoryHandlers;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void RegistOnReceiveTrData(object sender, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string[] parseRqName = e.sRQName.Split(';');
            string TrCode = parseRqName[0];
            string Ticker = parseRqName[1];
            switch (TrCode)
            {
                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식주봉차트조회요청):

                    break;
                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식일봉차트조회요청):
                    OPT주식일봉차트조회요청(e, Ticker);
                    break;

                case nameof(KIWOOM_OPT_TR_CODE_DEF.주식분봉차트조회요청):

                    break;
            }
        }

        public void OPT주식일봉차트조회요청(_DKHOpenAPIEvents_OnReceiveTrDataEvent eventHandle, string code)
        {
            //주식일봉차트조회요청_Output i = 주식일봉차트조회요청_Output.종목코드;
            object[,]recvDatas = (object[,])GetCommDataEx(eventHandle.sTrCode,eventHandle.sRQName);
            List<CANDLE_STICK_DEF> datas = new List<CANDLE_STICK_DEF>();
            for(int row= 0; row < recvDatas.GetLength(0); row++)
            {
                CANDLE_STICK_DEF candleData = new CANDLE_STICK_DEF();
                candleData.EndBody = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.현재가]);
                candleData.Volume = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.거래량]);
                candleData.Time = (string)recvDatas[row, (int)주식일봉차트조회요청_Output.일자];
                candleData.StartBody = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.시가]);
                candleData.UpStroke = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.고가]);
                candleData.DownStroke = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.저가]);
                datas.Add(candleData);

                //주식차트조회요청 chartData = new 주식차트조회요청();
                //chartData.현재가 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.현재가]);
                //chartData.거래량 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.거래량]);
                //chartData.거래대금 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.거래대금]);
                //chartData.체결시간 = (string)recvDatas[row, (int)주식일봉차트조회요청_Output.일자];
                //chartData.시가 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.시가]);
                //chartData.고가 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.고가]);
                //chartData.저가 = int.Parse((string)recvDatas[row, (int)주식일봉차트조회요청_Output.저가]);
                //Debug.WriteLine($"{chartData.체결시간} : {chartData.현재가} {chartData.시가} {chartData.고가} {chartData.저가}");
            }
            CandleSticks storeData = new CandleSticks(code);
            storeData.AddChartCandleStick(datas.ToArray(),CANDLE_TIME_FRAME_DEF.DAY);
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
