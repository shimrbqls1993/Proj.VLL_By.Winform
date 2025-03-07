using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using Proj.VVL.Interfaces.Chart.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using Proj.VVL.Interfaces.DataInventoryHandlers;
using Proj.VVL.Data;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Proj.VVL.Behaviors.Common.CalcIndecator;
using System.Diagnostics;

namespace Proj.VVL.Interfaces.Chart
{
    public class ChartHandler
    {
        private CandleSticks savedCandleStickData;
        public CANDLE_DATA_DEF allCandleDatas;
        public CANDLE_STICK_DEF[] candleStickData;

        public ChartHandler(string stockCode, CANDLE_TIME_FRAME_DEF timeDiv)
        {
            savedCandleStickData = new CandleSticks(stockCode);
            allCandleDatas = savedCandleStickData.GetCandleDatasFromFile();
            switch (timeDiv)
            {
                case CANDLE_TIME_FRAME_DEF.MIN_1:
                    candleStickData = allCandleDatas.MIN_1;
                    break;
                case CANDLE_TIME_FRAME_DEF.MIN_5:
                    candleStickData = allCandleDatas.MIN_5;
                    break;
                case CANDLE_TIME_FRAME_DEF.HOUR:
                    candleStickData = allCandleDatas.HOUR;
                    break;
                case CANDLE_TIME_FRAME_DEF.DAY:
                    candleStickData = allCandleDatas.DAY;
                    break;
                case CANDLE_TIME_FRAME_DEF.WEEK:
                    candleStickData = allCandleDatas.WEEK;
                    break;
            }
        }

        private double GetMaxVolume()
        {
            double result = 0;
            if (candleStickData != Array.Empty<CANDLE_STICK_DEF>())
            {
                for (int i = 0; i < candleStickData.Length; i++)
                {
                    if (result < candleStickData[i].Volume)
                    {
                        result = candleStickData[i].Volume;
                    }
                }
                return result;
            }
            return -1;
        }

        private double GetMaxPrice()
        {
            double result = 0;
            if (candleStickData != Array.Empty<CANDLE_STICK_DEF>())
            {
                for(int i =0; i<candleStickData.Length; i++)
                {
                    if(result < candleStickData[i].High)
                    {
                        result = candleStickData[i].High;
                    }
                }
                return result;
            }
            return -1;
        }

        private double GetMinPrice(double maxPrice)
        {
            double result = maxPrice;
            if (candleStickData != Array.Empty<CANDLE_STICK_DEF>())
            {
                for (int i = 0; i < candleStickData.Length; i++)
                {
                    if (result > candleStickData[i].Low)
                    {
                        result = candleStickData[i].Low;
                    }
                }
                return result;
            }
            return -1;
        }
        /// <summary>
        /// 조금 더 채널 형태로 깍아내기 필요하다.
        /// </summary>
        private void movingAverTest()
        {
            allCandleDatas.MOVING_AVR.DAY = CalcMovingAverage.Result(allCandleDatas.DAY, 1);

            int startBuff = 0;
            int endBuff = 0;
            for(int i =0; i<allCandleDatas.MOVING_AVR.DAY.HighPrices.Length; i++)
            {
                startBuff = 0;
                endBuff = 0;
                for(int j = 0; j<allCandleDatas.HOUR.Length; j++)
                {
                    if (allCandleDatas.HOUR[j].Datetime.StartsWith(allCandleDatas.MOVING_AVR.DAY.DateTime[i]))
                    {
                        if(startBuff == 0 && endBuff == 0)
                        {
                            startBuff = j;
                            endBuff = j;
                        }
                        else
                        {
                            endBuff++;
                        }
                    }
                }
                double dtBuff = endBuff - startBuff;
                if(dtBuff == 0)
                {
                    allCandleDatas.HOUR[startBuff].ShowMovingAvrHigh = allCandleDatas.MOVING_AVR.DAY.HighPrices[i];
                    allCandleDatas.HOUR[startBuff].ShowMovingAvrLow = allCandleDatas.MOVING_AVR.DAY.LowPrices[i];
                }
                else if(dtBuff > 0)
                {
                    double dtHPrice = allCandleDatas.MOVING_AVR.DAY.HighPrices[i];
                    double dtLPrice = allCandleDatas.MOVING_AVR.DAY.LowPrices[i];
                    if (i != (allCandleDatas.MOVING_AVR.DAY.HighPrices.Length - 1))
                    {
                        dtHPrice = (allCandleDatas.MOVING_AVR.DAY.HighPrices[i + 1] - allCandleDatas.MOVING_AVR.DAY.HighPrices[i]) / dtBuff;
                        dtLPrice = (allCandleDatas.MOVING_AVR.DAY.LowPrices[i + 1] - allCandleDatas.MOVING_AVR.DAY.LowPrices[i]) / dtBuff;
                    }
                    else
                    {
                        dtHPrice = 0;
                        dtLPrice = 0;
                    }
                    for(int j = 0; j<=dtBuff; j++)
                    {
                        allCandleDatas.HOUR[startBuff + j].ShowMovingAvrHigh = allCandleDatas.MOVING_AVR.DAY.HighPrices[i] + (dtHPrice * j);
                        allCandleDatas.HOUR[startBuff + j].ShowMovingAvrLow = allCandleDatas.MOVING_AVR.DAY.LowPrices[i] + (dtLPrice * j);
                    }
                }
                else
                {
                    Debug.WriteLine("구조가 이상함!!!!!");
                }
            }

            candleStickData = allCandleDatas.HOUR;
            //하단 채널
            savedCandleStickData.ReWriteCandleData(allCandleDatas);
        }

        public LiveChartProperties GetCandleData()
        {
            movingAverTest();
            LiveChartProperties chartProperties = new LiveChartProperties();

            if (candleStickData != Array.Empty<CANDLE_STICK_DEF>())
            {
                double maxPrice = GetMaxPrice();
                double minPrice = GetMinPrice(maxPrice);
                double maxVolume = GetMaxVolume();

                chartProperties.Series =
                new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Values = candleStickData.Select( x => x.Volume).ToArray(),
                        MaxBarWidth = 10,
                        ScalesYAt = 1,
                    },
                    new CandlesticksSeries<FinancialPointI>
                    {
                        Values = candleStickData.Select( x => new FinancialPointI(x.High,x.Open,x.Close,x.Low)).ToArray(),
                        ScalesYAt = 0,
                    },
                    new LineSeries<double>
                    {
                        //Values = allCandleDatas.MOVING_AVR.WEEK.HighPrices,
                        Values = candleStickData.Select( x => x.ShowMovingAvrHigh).ToArray(),
                        Fill = null,
                        GeometrySize = 0
                    },
                    new LineSeries<double>
                    {
                        //Values = allCandleDatas.MOVING_AVR.WEEK.LowPrices,
                        Values = candleStickData.Select( x => x.ShowMovingAvrLow).ToArray(),
                        Fill = null,
                        GeometrySize = 0
                    }
                };

                chartProperties.xAxes = new[]
                {
                    new Axis
                    {
                        Labels = candleStickData.Select(x=>x.Datetime.Insert(4,"-").Insert(7,"-")).ToArray(),
                        LabelsRotation = 15,
                        ShowSeparatorLines = true,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                    }
                };


                chartProperties.yAxes = new[]
                {
                    new Axis
                    {
                        Name = "가격",
                        //MaxLimit = maxPrice + (0.1*maxPrice),
                        //MinLimit = minPrice + (0.1*minPrice),
                        Position = LiveChartsCore.Measure.AxisPosition.Start
                    },
                    new Axis
                    {
                        Name = "거래량",
                        MaxLimit = maxVolume * 5,
                        MinLimit = 0,
                        IsVisible = false,
                        Position = LiveChartsCore.Measure.AxisPosition.End
                    }
                };


                return chartProperties;
            }
            else
            {
                return null;
            }
        }


    }
}
