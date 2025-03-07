using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcRoundFigure
    {
        public CANDLE_DATA_DEF candleData { get; set; }
        

        public CalcRoundFigure(double[] price) 
        {
            
        }

        /// <summary>
        /// 호가가 1원이면 10원 50원
        /// 호가가 10원이면 100원 500원 1000원
        /// 호가가 100원이면 1000원 5000원 10000원
        /// 1000원 미만은 50, 100월
        /// 1000~10000 500,1000원
        /// 10000 ~ 100000
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        //private int KoreaStockRoundFigureLevel(double[] prices)
        //{
        //    List<double> hogas = new List<double>();
        //    double prevPrices = 0;
        //    for (int i = 0; i<prices.Length; i++)
        //    {
        //        if(i == 0)
        //        {
        //            prevPrices = prices[i];
        //        }
        //        else
        //        {
        //            hogas.Add(prices[i] - prevPrices);
        //        }
        //    }
            
        //    foreach(double hoga in hogas)
        //    {

        //    }
        //}
    }

    public class KoreaStockRoundFigureLevel
    {
        public KoreaStockRoundFigureLevel()
        {

        }
    }
}
