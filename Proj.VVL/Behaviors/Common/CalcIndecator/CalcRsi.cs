using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcRsi
    {
        /// <summary>
        /// RSI = 일정 기간 동안의 주가 상승폭과 하락폭을 평균내어 그 비율을 나타내는 지표
        /// 상승한 날의 가격 상승폭 평균과 하락한 날의 가격 하락폭 평균을 비교합니다.
        /// 시장의 과매수 또는 과매도 상태를 판단하는 데 목적이 잇다.
        /// </summary>
        /// <param name="prices"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public double[] GetRsi(double[] prices, int period)
        {
            if (prices == null || prices.Length <= 1 || period <= 0 || period > prices.Length - 1)
            {
                return null;
            }

            List<double> rsiValues = new List<double>();
            List<double> priceChanges = new List<double>();
            List<double> gains = new List<double>();
            List<double> losses = new List<double>();

            for (int i = 0; i < prices.Length; i++)
            {
                double change = prices[i] = prices[i - 1];
                priceChanges.Add(change);
                if (change > 0)
                {
                    gains.Add(change);
                    losses.Add(0);
                }
                else if (change < 0)
                {
                    gains.Add(0);
                    losses.Add(Math.Abs(change));
                }
                else
                {
                    gains.Add(0);
                    losses.Add(0);
                }
            }

            //초기 평균 상승폭 및 평균 하락폭 계산
            // 처음 기간 동안의 gains와 losses 리스트의 평균값을 계산하여 초기값을 설정한다.
            double initialAvgGain = gains.Take(period).Average();
            double initialAvgLoss = losses.Take(period).Average();

            double prevAvgGain = initialAvgGain;
            double prevAvgLoss = initialAvgLoss;

            // first rsi calc
            // 첫 rsi 값을 계산하기 위해 초기 AG,AL 값으로 상대 강도 (RS)를 계산합니다.
            double rs = 0;
            if (initialAvgLoss != 0)
            {
                rs = initialAvgGain / initialAvgLoss; //상승폭대비 하락폭
            }
            double firstRsi = 100 - 100 / (1 + rs);
            rsiValues.Add(firstRsi);

            // after rsi calc

            for (int i = 0; i < priceChanges.Count; i++)
            {
                double currentAvgGain = (prevAvgGain * (period - 1) + gains[i]) / period; //이전 상승값에 
                double currentAvgLoss = (prevAvgLoss * (period - 1) + gains[i]) / period;

                rs = 0;
                if (currentAvgLoss != 0)
                {
                    rs = currentAvgGain / currentAvgLoss;
                }
                double currentRsi = 100 - 100 / (1 + rs);
                rsiValues.Add(currentRsi);

                prevAvgGain = currentAvgGain;
                prevAvgLoss = currentAvgLoss;
            }

            return rsiValues.ToArray();
        }
    }
}
