using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcMovingAverage
    {
        //double[] Prices;
        //int Period;

        //public CalcMovingAverage(double[] priceDatas, int period)
        //{
        //    Prices = priceDatas;
        //    Period = period;
        //}


        public static MOVING_AVR_PROPERTIES Result(CANDLE_STICK_DEF[] candleDatas, int period)
        {
            List<double> highPrices = new List<double>();
            List<double> lowPrices = new List<double>();
            List<double> openPrices = new List<double>();
            List<double> closePrices = new List<double>();
            List<string> dateTimes = new List<string>();
            List<string> syncPriceLevel = new List<string>();

            //상단 채널
            foreach (CANDLE_STICK_DEF candleData in candleDatas)
            {
                highPrices.Add(candleData.High);
                lowPrices.Add(candleData.Low);
                openPrices.Add(candleData.Open);
                closePrices.Add(candleData.Close);
            }

            MOVING_AVR_PROPERTIES result = new MOVING_AVR_PROPERTIES();
            result.HighPrices = Result(highPrices.ToArray(), period);
            result.LowPrices = Result(lowPrices.ToArray(), period);
            result.OpenPrices = Result(openPrices.ToArray(), period);
            result.ClosePrices = Result(closePrices.ToArray(), period);

            //List<MOVING_AVR_PROPERTIES> result = new List<MOVING_AVR_PROPERTIES>();
            //highPrices = Result(highPrices.ToArray(), period).ToList();
            //lowPrices = Result(lowPrices.ToArray(), period).ToList();
            //openPrices = Result(openPrices.ToArray(), period).ToList();
            //closePrices = Result(closePrices.ToArray(), period).ToList();

            //sync datetime
            for (int i = period - 1; i < candleDatas.Length; i++)
            {
                dateTimes.Add(candleDatas[i].Datetime);
            }

            //int j = 0;
            //TimeSpan dateTimeUnit = CommonFunc.ParseString2DateTime(compCandleDatas[1].Datetime, Define.ShortStr2DateTimeFormat) - CommonFunc.ParseString2DateTime(compCandleDatas[0].Datetime, Define.ShortStr2DateTimeFormat);
            //for (int i = 0; i < compCandleDatas.Length; i++)
            //{
            //    DateTime movingAvrDt = CommonFunc.ParseString2DateTime(dateTimes[j], Define.ShortStr2DateTimeFormat);
            //    DateTime compDt = CommonFunc.ParseString2DateTime(compCandleDatas[i].Datetime, Define.ShortStr2DateTimeFormat);

            //    if (movingAvrDt <= compDt)
            //    {
            //        TimeSpan ts = compDt - movingAvrDt;
            //        if (ts >= dateTimeUnit)
            //        {
            //            j++;
            //        }
            //        else
            //        {
            //            if ()

            //                result.Add(new MOVING_AVR_PROPERTIES() { HighPrices })
            //        }
            //    }
            //    compCandleDatas[i].Datetime
            //}

            result.DateTime = dateTimes.ToArray();

            return result;
        }

        /// <summary>
        /// Datatime Offset으로 인해 앞에s
        /// </summary>
        /// <returns></returns>
        public static double[] Result(double[] Prices, int Period)
        {
            if (Prices == null || Prices.Length == 0 || Period <= 0 || Period > Prices.Length)
            {
                return null;
            }
            List<double> result = new List<double>();

            for (int i = Period - 1; i < Prices.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < Period; j++)
                {
                    sum += Prices[i - j];
                }
                result.Add(sum / Period);
            }

            return result.ToArray();
        }
    }
}
