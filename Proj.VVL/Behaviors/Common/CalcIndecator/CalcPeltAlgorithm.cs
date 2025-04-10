using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcPeltAlgorithm
    {
        private double penalty;
        private double[] prices;

        public CalcPeltAlgorithm(double[] Prices, double Penalty)
        {
            prices = Prices;
            penalty = Penalty;
        }

        public List<int> DetectChangePoints()
        {
            int n = prices.Length;
            double[] F = new double[n + 1]; //최소 비용 저장 배열
            List<int>[] Result = new List<int>[n+1]; //각 위치에서의 변동점 리스트

            //초기화
            for(int t =0; t <= n; t++)
            {
                F[t] = double.PositiveInfinity;
                Result[t] = new List<int>();
            }
            F[0] = 0;

            //계산
            for(int t = 1; t <= n; t++)
            {
                for(int s =0; s < t; s++)
                {
                    double[] segment = new double[t - s];
                    Array.Copy(prices, s, segment, 0, t - s);

                    double cost = F[s] + SumOfSquaredErrors(segment) + penalty;
                    Debug.WriteLine($"cost : {cost}");
                    if(cost < F[t])
                    {
                        F[t] = cost;
                        Result[t] = new List<int>(Result[s]);
                        Result[t].Add(s);
                    }
                }

                //가지치기
                List<int> toRemove = new List<int>();
                foreach(int s in Result[t])
                {
                    double[] segment1 = new double[t - s];
                    Array.Copy(prices, s, segment1, 0, t - s);
                    double cost1 = F[s] + SumOfSquaredErrors(segment1);

                    for(int u=t + 1; u <= n; u++)
                    {
                        double[] segment2 = new double[u - t];
                        Array.Copy(prices, t, segment2, 0, u - t);
                        double cost2 = F[t] + SumOfSquaredErrors(segment2);

                        if(cost1 + SumOfSquaredErrors(segment2) > cost2)
                        {
                            toRemove.Add(s);
                            break;
                        }
                    }
                }

                foreach(int s in toRemove)
                {
                    Result[t].Remove(s);
                }
            }

            return Result[n];
        }

        

        public double SumOfSquaredErrors(double[] segment)
        {
            if (segment.Length == 0) return 0;

            double average = 0;
            foreach(double val in segment)
            {
                average += val;
            }
            average /= segment.Length;

            double result = 0;
            foreach(double val in segment)
            {
                result += (val - average) * (val - average);
            }
            return result;
        }
    }
}
