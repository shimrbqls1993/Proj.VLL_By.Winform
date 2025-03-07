using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Model.RecommandTickerModel;
using Proj.VVL.Interfaces.PubSub;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using System.Diagnostics;
using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Behaviors.Common.CalcIndecator;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.DataInventoryHandlers;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class PublishTickerManager : ServiceManagerBase
    {
        public ObservableCollection<Ticker> RecommandKoreaTickers = new ObservableCollection<Ticker>();
        public ObservableCollection<Ticker> RecommandUSTickers = new ObservableCollection<Ticker>();
        public ObservableCollection<Ticker> RecommandCryptoTickers = new ObservableCollection<Ticker>();
        public static Dictionary<string, int> Ticker_ThreadId = new Dictionary<string, int>();

        public PublishTickerManager(IRecommandTickerHandler hRecommandTicker) 
        {
            RecommandKoreaTickers = hRecommandTicker.LoadData();
        }

        public void Start()
        {
            IsRunning = true;
            Handler = new Thread(Main);
            Handler.Start();
        }

        public void Stop() 
        {
            IsRunning = false;
            Handler.Join(2000);
        }

        public void Main()
        {
            while (IsRunning) 
            {
                try
                {
                    if(MainForm.KiwoomOcxObj.login.GetConnectState() == KIWOOM_STATE_DEF.OK)
                    {
                        foreach (Ticker registedTicker in RecommandKoreaTickers)
                        {
                            if(registedTicker.Code != null)
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(StartTrading), registedTicker.Code);
                            }
                        }
                        return;
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// 1. 유효하지 않은 티커면은 자동 삭제
        /// 2. 마칠 때 확실하게 마치기
        /// 3. thread priority를 동적으로 하기
        /// 4. threadid 저장해서 상태 확인
        /// </summary>
        private static void StartTrading(object? tickerCode) 
        {
            // 1
            int initializeResult = StartTradingInit((string) tickerCode);
            if(initializeResult == -1)
            {
                return;
            }

            StartTradingMain(initializeResult);

            StopTrading((string) tickerCode);
        }

        private static int StartTradingInit(string tickerCode)
        {

            if (!int.TryParse(tickerCode, out int intTickerCode))
            {
                Debug.WriteLine("ticker code가 int형이 아닙니다." + tickerCode);
                return -1;
            }

            if (Ticker_ThreadId.TryGetValue(tickerCode, out int threadId))
            {
                Debug.WriteLine($"이미 등록되어 있는 티커 입니다. {tickerCode}");
                return -1;
            }

            Subscriber getChartData = new Subscriber();
            if (getChartData.GetKiwoomCandleData(tickerCode, Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.WEEK) != ERROR_CODE_DEF.NONE)
            {
                Debug.WriteLine($"Get data failed");
                return -1;
            }
            if (getChartData.GetKiwoomCandleData(tickerCode, Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.DAY) != ERROR_CODE_DEF.NONE)
            {
                Debug.WriteLine($"Get data failed");
                return -1;
            }
            if (getChartData.GetKiwoomCandleData(tickerCode, Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.HOUR) != ERROR_CODE_DEF.NONE)
            {
                Debug.WriteLine($"Get data failed");
                return -1;
            }
            // 생각해보자.
            if (getChartData.GetKiwoomCandleData(tickerCode, Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.MIN_5) != ERROR_CODE_DEF.NONE)
            {
                Debug.WriteLine($"Get data failed");
                return -1;
            }
            if (getChartData.GetKiwoomCandleData(tickerCode, Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.MIN_1) != ERROR_CODE_DEF.NONE)
            {
                Debug.WriteLine($"Get data failed");
                return -1;
            }

            CandleSticks getCandleDatas = new CandleSticks(tickerCode);
            CANDLE_DATA_DEF candleDatas = getCandleDatas.GetCandleDatasFromFile();

            CalcMovingAverage.Result(candleDatas.DAY,5);
            //하단 채널
            getCandleDatas.ReWriteCandleData(candleDatas);

            // 4
            Ticker_ThreadId.Add(tickerCode, Thread.CurrentThread.ManagedThreadId);

            return intTickerCode;
        }

        /// <summary>
        /// 1. 티커가 추가 되면
        /// 2. 차트 데이터를 수집한다.
        /// 3. 시간봉은 타이머를 주어서 시간봉이 뜰 때 되면 추가한다.
        /// 4. 외에는 실시간으로 데이터를 받아서 해당 선까지 내려오는지 확인
        /// 5. 각 일자별로 이평선을 상하단 채널을 구해 이평선 중 각 차트 데이터의 상단 하단을 채널로 하는 10일선 데이터를 구한다.
        /// 6. 각 일자별 
        /// </summary>
        /// <param name="tickerCode"></param>
        private static void StartTradingMain(int tickerCode)
        {

        }

        private static void StopTrading(string tickerCode) 
        {
            //2
            Ticker_ThreadId.Remove(tickerCode);
        }
    }
}
