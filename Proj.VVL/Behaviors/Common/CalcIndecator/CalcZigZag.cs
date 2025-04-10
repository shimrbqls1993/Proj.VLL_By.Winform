using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcZigZag
    {
        public CalcZigZag()
        {

        }

        public List<ZigZagPoint> Calculated(double[] prices, double thresholdPercent)
        {
            List<ZigZagPoint> point = new List<ZigZagPoint>();
            if(prices.Length < 2) return point;
            double lastPrice = prices[0];
            bool isRising = true;
            double lastExtreme = lastPrice;

            for (int i = 1; i < prices.Length; i++)
            {
                double changePercent = Math.Abs((prices[i] - lastExtreme) / lastExtreme) * 100;
                if (changePercent >= thresholdPercent) 
                {
                    if(isRising && prices[i] < lastExtreme)
                    {
                        point.Add(new ZigZagPoint { Index = i - 1, Price = lastExtreme, IsPeak = true });
                        isRising = false;
                    }
                    else if(isRising && prices[i] > lastExtreme)
                    {
                        point.Add(new ZigZagPoint { Index = i - 1, Price = lastExtreme, IsPeak = false });
                        isRising = true;
                    }
                    lastExtreme = prices[i];
                }
            }

            if(point.LastOrDefault()?.Price != lastExtreme)
            {
                point.Add(new ZigZagPoint { Index = prices.Length - 1, Price = lastExtreme, IsPeak = isRising });
            }

            return point;
        }
    }

    public class ZigZagPoint()
    {
        public int Index { get; set; }
        public double Price { get; set; }
        public bool IsPeak { get; set; }
    }
}
