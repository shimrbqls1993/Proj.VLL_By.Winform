using Proj.VVL.Data;
using Proj.VVL.Interfaces.DataInventoryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcIndecatorsBehaviors
    {
        public static double[] MovingAverage(double[] prices, int period)
        {
            if (prices == null || prices.Length == 0 || period <= 0 || period > prices.Length)
            {
                return null;
            }

            List<double> result = new List<double>();

            for (int i = period - 1; i < prices.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < period; j++)
                {
                    sum += prices[i - j];
                }
                result.Add(sum / period);
            }

            return result.ToArray();
        }
    }
}
