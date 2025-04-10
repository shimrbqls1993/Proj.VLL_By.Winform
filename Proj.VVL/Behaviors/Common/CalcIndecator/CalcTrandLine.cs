using MathNet.Numerics;
using Newtonsoft.Json;
using Proj.VVL.Behaviors.Abstractions;
using Proj.VVL.Data;
using System.Diagnostics;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    /// <summary>
    /// 1. 현재 타임프레임에서 추세 자체가 우상향인지 알 수 있다.
    /// </summary>
    public class CalcTrandLine : CalcIndecatorBase
    {

        const double defaultChannelMultiple = 2;
        const double MaxRsquared = 2;

        private List<double> GetBestTrandLine(List<double> baseTrandLine, List<double> targetPrice, double rSquaredOffset, double StandardDeviation, bool isSum)
        {
            List<double> bestResult = new List<double>();
            double channelMultiple = defaultChannelMultiple;
            double bestCannelMultipleSearchUnit = 0.01;
            List<double> allRsquared = new List<double>();
            while (channelMultiple > 1)
            {
                bestResult.Clear();
                for (int i = 0; i < baseTrandLine.Count; i++)
                {
                    if (isSum)
                    {
                        bestResult.Add(baseTrandLine[i] + (StandardDeviation * channelMultiple));
                    }
                    else
                    {
                        bestResult.Add(baseTrandLine[i] - (StandardDeviation * channelMultiple));
                    }
                }

                double rSquared = CalcCustomRsquared(targetPrice, bestResult, rSquaredOffset);
                allRsquared.Add(rSquared);
                Debug.WriteLine($"High Rs is {rSquared} and channelMultiple {channelMultiple}");
                channelMultiple -= bestCannelMultipleSearchUnit;
            }

            // search best R-Squared
            // Best R-Squared parameter 적용
            double bestRsquared = allRsquared.Max();
            double bestRsquaredIndex = allRsquared.IndexOf(bestRsquared);
            channelMultiple = defaultChannelMultiple - (bestCannelMultipleSearchUnit * bestRsquaredIndex);
            bestResult.Clear();
            Debug.WriteLine($"best channel multiple : {channelMultiple} / R squared : {bestRsquared}");
            Debug.WriteLine($"R squared offset is {rSquaredOffset}");
            for (int i = 0; i < baseTrandLine.Count; i++)
            {
                if (isSum)
                {
                    bestResult.Add(baseTrandLine[i] + (StandardDeviation * channelMultiple));
                }
                else
                {
                    bestResult.Add(baseTrandLine[i] - (StandardDeviation * channelMultiple));
                }
            }
            return bestResult;
            // 2차 결과값인데 bestResult랑 거의 흡사하게 나옴
            /*
            allRsquared.RemoveAt((int)bestRsquaredIndex);
            bestRsquared = allRsquared.Max();
            bestRsquaredIndex = allRsquared.IndexOf(bestRsquared);
            List<double> goodResult = new List<double>();
            if(bestRsquared < 0.1)
            {
                return (bestResult, goodResult);
            }
            channelMultiple = defaultChannelMultiple - (bestCannelMultipleSearchUnit * bestRsquaredIndex);
            for (int i = 0; i < baseTrandLine.Count; i++)
            {
                if (isSum)
                {
                    goodResult.Add(baseTrandLine[i] + (StandardDeviation * channelMultiple));
                }
                else
                {
                    goodResult.Add(baseTrandLine[i] - (StandardDeviation * channelMultiple));
                }
            }
            Debug.WriteLine($"good channel multiple : {channelMultiple} / R squared : {bestRsquared}");
            Debug.WriteLine($"R squared offset is {rSquaredOffset}");
            return (bestResult, goodResult);
            */
        }

        /// <summary>
        /// 문제
        /// channel multiple이 올라갈 때마다 R-Squared값이 올라가는 경향이 있음
        /// </summary>
        /// <param name="datas"></param>
        public void Result(ref CANDLE_STICK_DEF[] datas)
        {
            List<double> time = new List<double>();
            List<double> closePrice = new List<double>();
            List<double> trandLineClosePrice = new List<double>();
            List<double> highPrice = new List<double>();
            List<double> predictHighPrice = new List<double>();
            List<double> predictHigh2Price = new List<double>();
            List<double> lowPrice = new List<double>();
            List<double> predictLowPrice = new List<double>();
            List<double> predictLow2Price = new List<double>();

            //1. 기본적으로 알맞는 값 집어넣기
            for (int i = 0; i < datas.Length; i++)
            {
                time.Add(i);
                closePrice.Add(datas[i].Close);
                highPrice.Add(datas[i].High);
                lowPrice.Add(datas[i].Low);
            }
            //2. Close 값 선형회귀 파라미터 계산
            (double intercept, double slope) = Fit.Line(time.ToArray(), closePrice.ToArray());
            //3. 방정식에 선형회귀 파라미터 넣어서 결과 도출
            for (int i = 0; i < datas.Length; i++)
            {
                trandLineClosePrice.Add(intercept + (slope * i));
            }
            //4. 오차율 계산
            double stdDev = CalcStandardDeviation(closePrice, trandLineClosePrice);

            double rSquaredOffset = CalcAverageTrueRange.Result_AveragePercent(datas);
            if (rSquaredOffset > MaxRsquared)
            {
                Debug.WriteLine($"R-Squared Result is {rSquaredOffset}");
                Debug.WriteLine($"R-Squared {rSquaredOffset} -> {MaxRsquared}");
                rSquaredOffset = MaxRsquared;
            }
            predictHighPrice = GetBestTrandLine(trandLineClosePrice, highPrice, rSquaredOffset, stdDev, true);
            predictLowPrice = GetBestTrandLine(trandLineClosePrice, lowPrice, rSquaredOffset, stdDev, false);

            for (int i = 0; i < predictHighPrice.Count; i++)
            {
                datas[i].ShowMovingAvrHigh = predictHighPrice[i];
                datas[i].ShowMovingAvrLow = predictLowPrice[i];
            }
        }
    }
}
