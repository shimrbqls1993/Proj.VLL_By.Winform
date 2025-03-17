using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    /// <summary>
    /// 변동성 측정
    /// </summary>
    public class CalcAverageTrueRange
    {
        public static List<double> Result(CANDLE_STICK_DEF[] datas, int period = 14)
        {
            if (datas == null || datas.Length < period || period <= 0)
            {
                return new List<double>(); // 유효하지 않은 입력
            }

            List<double> atrValues = new List<double>();
            List<double> trueRanges = new List<double>();

            // 첫 번째 True Range 계산
            for (int i = 1; i < datas.Length; i++)
            {
                double high = datas[i].High;
                double low = datas[i].Low;
                double prevClose = datas[i - 1].Close;

                double tr = Math.Max(high - low, Math.Abs(high - prevClose));
                tr = Math.Max(tr, Math.Abs(low - prevClose));
                trueRanges.Add(tr);
            }

            // 첫 번째 ATR 값 계산 (Simple Moving Average)
            if (trueRanges.Count >= period)
            {
                double firstAtr = trueRanges.Take(period).Average();
                atrValues.Add(firstAtr);

                // 이후 ATR 값 계산 (Smoothed Moving Average)
                for (int i = period; i < trueRanges.Count; i++)
                {
                    double currentTr = trueRanges[i];
                    double previousAtr = atrValues.Last();
                    double currentAtr = ((previousAtr * (period - 1)) + currentTr) / period;
                    atrValues.Add(currentAtr);
                }
            }

            return atrValues;
        }

        public static double Result_AveragePercent(CANDLE_STICK_DEF[] datas, int period = 14)
        {
            List<double> atrDatas = Result(datas, period);
            return (atrDatas.Average() / datas.Last().Close) * 100;
        }
    }
}
