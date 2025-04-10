using Proj.VVL.Behaviors.Abstractions;
using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    /// <summary>
    /// 차트의 추세 변화와 잠재적인 지지/저항 수준을 판단
    /// 당일 매매의 저항과 지지선을 미리 예측하고 가격 움직임에 대응
    /// 당일 매매에 유효한 전략이다.
    /// </summary>
    public class CalcPivotPoint : CalcIndecatorBase
    {
        List<CANDLE_STICK_DEF> Datas = new List<CANDLE_STICK_DEF>();
        public CalcPivotPoint(CANDLE_STICK_DEF[] data) 
        {
            Datas = data.ToList();
        }

        public List<PIVOT_POINT_RESULT_DEF> Standard()
        {
            List<PIVOT_POINT_RESULT_DEF> result = new List<PIVOT_POINT_RESULT_DEF>();
            foreach (CANDLE_STICK_DEF data in Datas)
            {
                PIVOT_POINT_RESULT_DEF pp = new PIVOT_POINT_RESULT_DEF();
                pp.datetime = CommonFunc.ParseString2DateTime(data.Datetime);
                double p = (data.Close + data.Low + data.High) / 3;

                pp.supportLine1 = (p * 2) - data.High;
                pp.supportLine2 = p - (data.High - data.Low);
                pp.registLine1 = (p * 2) - data.Low;
                pp.registLine2 = p + (data.High - data.Low);

                result.Add(pp);
            }
            return result;
        }

        public List<PIVOT_POINT_RESULT_DEF> Fibonacci()
        {
            List<PIVOT_POINT_RESULT_DEF> result = new List<PIVOT_POINT_RESULT_DEF>();
            foreach (CANDLE_STICK_DEF data in Datas)
            {
                PIVOT_POINT_RESULT_DEF pp = new PIVOT_POINT_RESULT_DEF();
                pp.datetime = CommonFunc.ParseString2DateTime(data.Datetime);
                double p = (data.Close + data.Low + data.High) / 3;

                pp.supportLine1 = p - (0.382 * (data.High - data.Low));
                pp.supportLine2 = p - (0.618 * (data.High - data.Low));
                pp.supportLine3 = p - (1 * (data.High - data.Low));
                pp.supportLine4 = p - (1.618 * (data.High - data.Low));
                pp.registLine1 = p + (0.382 * (data.High - data.Low));
                pp.registLine2 = p + (0.618 * (data.High - data.Low));
                pp.registLine3 = p + (1 * (data.High - data.Low));
                pp.registLine4 = p + (1.618 * (data.High - data.Low));

                result.Add(pp);
            }
            return result;
        }
    }

    public class PIVOT_POINT_RESULT_DEF
    {
        public DateTime datetime { get; set; }
        public double supportLine1 { get; set; }
        public double supportLine2 { get; set; }
        public double supportLine3 { get; set; }
        public double supportLine4 { get; set; }
        public double registLine1 { get; set; }
        public double registLine2 { get; set; }
        public double registLine3 { get; set; }
        public double registLine4 { get; set; }
    }
}
