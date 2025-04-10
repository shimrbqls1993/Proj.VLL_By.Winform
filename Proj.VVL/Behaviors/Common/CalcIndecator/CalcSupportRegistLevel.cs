using Proj.VVL.Behaviors.Abstractions;
using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    /// <summary>
    /// 1 가격을 기반으로 저항과 지지선을 계산
    /// 2 거래량으로 1에서 찾은 선을 검증 및 필터
    /// 
    /// 라운드 피겨 가격대를 먼저 계산하고, 가격의 겹침을 확인
    /// 가격의 겹침을 먼저 확인하고 라운드 피겨를 확인하면 로직이 복잡해지지만 더 확실 할 수는 있을지도?
    /// </summary>
    public class CalcSupportRegistLevel : CalcIndecatorBase
    {
        List<CANDLE_STICK_DEF> Datas;
        List<double> highPrice = new List<double>();
        List<double> openPrice = new List<double>();
        List<double> closePrice = new List<double>();
        List<double> lowPrice = new List<double>();

        public CalcSupportRegistLevel(CANDLE_STICK_DEF[] datas) 
        {
            Datas = datas.ToList();
            foreach(CANDLE_STICK_DEF data in datas)
            {
                highPrice.Add(data.High);
                openPrice.Add(data.Open);
                closePrice.Add(data.Close);
                lowPrice.Add(data.Low);
            }
        }

        const double minimunRsquaredThreshold = 0.1;
        /// <summary>
        /// 최 저점부터 시작하여 특정 가격(저점,고점,종가)에 얼마나 겹치는지 확인한다.
        /// </summary>
        public List<double> SupportLine()
        {
            List<(double, double)> tablePrice_Rsquared = new List<(double, double)>();

            double Rsquared = 0;
            double startPrice = Datas[0].Low;
            double unitPrice = GetKoreaHogaUnitPrice(startPrice);
            double rSquaredOffset = CalcAverageTrueRange.Result_AveragePercent(Datas.ToArray());

            while (startPrice <= highPrice.Max())
            {
                Rsquared = 0;
                Rsquared += CalcCustomRsquared(highPrice, startPrice, rSquaredOffset);
                Rsquared += CalcCustomRsquared(openPrice, startPrice, rSquaredOffset);
                Rsquared += CalcCustomRsquared(closePrice, startPrice, rSquaredOffset);
                Rsquared += CalcCustomRsquared(lowPrice, startPrice, rSquaredOffset);
                tablePrice_Rsquared.Add((startPrice, Rsquared));
                startPrice += unitPrice;
            }

            List<(double, double)> result = tablePrice_Rsquared.OrderByDescending(item => item.Item2).ToList();
            // bestRsquared 기준으로 걸러내기 + 평균과 offset으로 걸러내기
            result = result.Where(item => item.Item2 > minimunRsquaredThreshold).ToList();
            // 평균의 절반
            //double RsquaredAvr = 0;
            //foreach (var test in result)
            //{
            //    RsquaredAvr += test.Item2;
            //}
            //RsquaredAvr /= result.Count;
            //result = result.Where(item => item.Item2 > RsquaredAvr).ToList();

            //Debug.WriteLine($"filterd average R sqared value is {RsquaredAvr}");
            int TopN = result.Count() / 10;
            result = result.Take(TopN).ToList();

            List<double> resultSolted = new List<double>();
            foreach (var test in result)
            {
                double roundFigurePrice = GetKoreaRoundFigure(test.Item1);
                if (!resultSolted.Contains(roundFigurePrice))
                {
                    Debug.WriteLine($"{test.Item1}, {test.Item2}");
                    resultSolted.Add(roundFigurePrice);
                    Debug.WriteLine($"Is Round Figure Level {roundFigurePrice}");
                }
            }

            // 집중도를 보이는 구간이 어느쪽인지

            // 거래량으로 마무리 확인
            Debug.WriteLine($"support line solted Len : {resultSolted.Count}");
            return resultSolted;
        }
    }
}
