using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Abstractions
{
    public class CalcIndecatorBase
    {
        public List<double> ChangedPercent(double[] prices)
        {
            List<double> Result = new List<double>();
            if (prices.Length < 2) return Result;

            Result.Add(0); //배열 순서를 맞춰주기 위함
            double lastPrice = prices[0];
            
            for(int i = 1; i<prices.Length; i++)
            {
                if ((prices[i] - lastPrice) >= 0) // positive
                    Result.Add(Math.Abs(((prices[i] - lastPrice) / lastPrice) * 100));
                else // negative
                    Result.Add(Math.Abs((((prices[i] - lastPrice) / lastPrice) * 100) * -1));

                lastPrice = prices[i];
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="predict"></param>
        /// <param name="offsetPercent"></param>
        /// 변동성과 관련된 지수가 들어가야할듯 함
        /// <returns></returns>
        public double CalcCustomRsquared(List<double> actual, List<double> predict, double offsetPercent = 0)
        {
            double correctCnt = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                double offset = CommonFunc.CalcPercent(actual[i], offsetPercent);
                double minActual = actual[i] - offset;
                double maxActual = actual[i] + offset;

                if (predict[i] <= maxActual && predict[i] >= minActual)
                {
                    correctCnt++;
                }
            }

            return (correctCnt / (double)actual.Count);
        }

        public double CalcCustomRsquared(List<double> actual, double predict, double offsetPercent = 0)
        {
            double correctCnt = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                double offset = CommonFunc.CalcPercent(actual[i], offsetPercent);
                double minActual = actual[i] - offset;
                double maxActual = actual[i] + offset;

                if (predict <= maxActual && predict >= minActual)
                {
                    correctCnt++;
                }
            }

            return (correctCnt / (double)actual.Count);
        }

        /// <summary>
        /// 표준 편차
        /// 예상 값이 실제값이랑 얼마나 차이가 나는지?
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="predicted"></param>
        /// <returns></returns>
        public double CalcStandardDeviation(List<double> actual, List<double> predicted)
        {
            if (actual.Count != predicted.Count || actual.Count == 0)
            {
                return 0;
            }

            double sumOfSquaredDifferences = 0;
            for (int i = 0; i < actual.Count; i++)
            {
                double difference = actual[i] - predicted[i];
                sumOfSquaredDifferences += difference * difference;
            }

            return Math.Sqrt(sumOfSquaredDifferences / actual.Count);
        }

        List<(double, double)> HogaUnitPriceTable = new List<(double, double)> {
            (2000, 1), //
            (5000, 5), //
            (20000, 10),
            (50000, 50), // 
            (200000, 100), // 500원
            (500000, 500), // 10000원
            (double.MaxValue, 1000), // 10000원?
        };

        public double GetKoreaHogaUnitPrice(double price)
        {
            for(int i =0; i< HogaUnitPriceTable.Count; i++)
            {
                if (price < HogaUnitPriceTable[i].Item1)
                {
                    return HogaUnitPriceTable[i].Item2;
                }
            }
            return 0;
        }

        public double GetKoreaRoundFigure(double price)
        {
            double roundFigureLevel = (GetKoreaHogaUnitPrice(price) * 10);
            double levelOffset = price % roundFigureLevel;
            if(levelOffset > 0)
            {
                return price - levelOffset + roundFigureLevel;
            }
            return price;
        }
    }
}
