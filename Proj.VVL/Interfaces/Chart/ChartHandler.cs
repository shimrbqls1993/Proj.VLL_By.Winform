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
using Microsoft.ML;
using MathNet.Numerics;

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
        /// 임펄스 확인 가능한 로직으로 생각됨
        /// </summary>
        private void movingAverImpulse(ref CANDLE_STICK_DEF[] longTimeFrame, ref MOVING_AVR_PROPERTIES longTimeFrameMA, ref CANDLE_STICK_DEF[] shortTimeFrame)
        {
            longTimeFrameMA = CalcMovingAverage.Result(longTimeFrame, 0);

            int startBuff = 0;
            int endBuff = 0;

            for (int i = 0; i < longTimeFrameMA.HighPrices.Length; i++)
            {
                startBuff = 0;
                endBuff = -1;
                //상위 time frame과 하위 timeframe 비교가 원활하게 하기 위해 string to Datetime 변환
                DateTime n0LongTimeFrame = CommonFunc.ParseString2DateTime(longTimeFrameMA.DateTime[i]);
                DateTime n1LongTimeFrame;
                if (i == longTimeFrameMA.HighPrices.Length - 1)
                {
                    n1LongTimeFrame = n0LongTimeFrame + TimeSpan.FromDays(14);
                }
                else
                {
                    n1LongTimeFrame = CommonFunc.ParseString2DateTime(longTimeFrameMA.DateTime[i + 1]);
                }
                //상위 time frame과 하위 timeframe의 비교 로직
                for (int j = 0; j < shortTimeFrame.Length; j++)
                {
                    DateTime dtShortTimeFrame = CommonFunc.ParseString2DateTime(shortTimeFrame[j].Datetime);
                    if (n0LongTimeFrame <= dtShortTimeFrame && n1LongTimeFrame > dtShortTimeFrame)
                    {
                        if (startBuff == 0 && endBuff == -1)
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

                //해당되는 shorttimeframe에 데이터를 넣음
                double dtBuff = endBuff - startBuff;
                if (dtBuff == 0)
                {
                    shortTimeFrame[startBuff].ShowMovingAvrHigh = longTimeFrameMA.HighPrices[i];
                    shortTimeFrame[startBuff].ShowMovingAvrLow = longTimeFrameMA.LowPrices[i];
                }
                else if (dtBuff > 0) // 중간
                {
                    double dtHPrice = longTimeFrameMA.HighPrices[i];
                    double dtLPrice = longTimeFrameMA.LowPrices[i];
                    if (i != (longTimeFrameMA.HighPrices.Length - 1))
                    {
                        dtHPrice = (longTimeFrameMA.HighPrices[i + 1] - longTimeFrameMA.HighPrices[i]) / (dtBuff + 1);
                        dtLPrice = (longTimeFrameMA.LowPrices[i + 1] - longTimeFrameMA.LowPrices[i]) / (dtBuff + 1);
                    }
                    else //마지막
                    {
                        dtHPrice = 0;
                        dtLPrice = 0;
                    }
                    for (int j = 0; j <= dtBuff; j++)
                    {
                        shortTimeFrame[startBuff + j].ShowMovingAvrHigh = longTimeFrameMA.HighPrices[i] + (dtHPrice * j);
                        shortTimeFrame[startBuff + j].ShowMovingAvrLow = longTimeFrameMA.LowPrices[i] + (dtLPrice * j);
                    }
                }
                else
                {
                    Debug.WriteLine("하위 프레임에서는 해당 시간이 없음.");
                }
            }

            candleStickData = shortTimeFrame;
            //하단 채널
            savedCandleStickData.ReWriteCandleData(allCandleDatas);
        }


        private void movingAverTest(ref CANDLE_STICK_DEF[] longTimeFrame, ref MOVING_AVR_PROPERTIES longTimeFrameMA, ref CANDLE_STICK_DEF[] shortTimeFrame)
        {
            longTimeFrameMA = CalcMovingAverage.Result(longTimeFrame, 0);

            int startBuff = 0;
            int endBuff = 0;

            for (int i = 0; i < longTimeFrameMA.HighPrices.Length; i++)
            {
                startBuff = 0;
                endBuff = -1;
                //상위 time frame과 하위 timeframe 비교가 원활하게 하기 위해 string to Datetime 변환
                DateTime n0LongTimeFrame = CommonFunc.ParseString2DateTime(longTimeFrameMA.DateTime[i]);
                DateTime n1LongTimeFrame;
                if (i == longTimeFrameMA.HighPrices.Length - 1)
                {
                    n1LongTimeFrame = n0LongTimeFrame + TimeSpan.FromDays(14);
                }
                else
                {
                    n1LongTimeFrame = CommonFunc.ParseString2DateTime(longTimeFrameMA.DateTime[i + 1]);
                }
                //상위 time frame과 하위 timeframe의 비교 로직
                for (int j = 0; j < shortTimeFrame.Length; j++)
                {
                    DateTime dtShortTimeFrame = CommonFunc.ParseString2DateTime(shortTimeFrame[j].Datetime);
                    if (n0LongTimeFrame <= dtShortTimeFrame && n1LongTimeFrame > dtShortTimeFrame)
                    {
                        if (startBuff == 0 && endBuff == -1)
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

                //해당되는 shorttimeframe에 데이터를 넣음
                double dtBuff = endBuff - startBuff;
                if (dtBuff == 0)
                {
                    shortTimeFrame[startBuff].ShowMovingAvrHigh = longTimeFrameMA.HighPrices[i];
                    shortTimeFrame[startBuff].ShowMovingAvrLow = longTimeFrameMA.LowPrices[i];
                }
                else if (dtBuff > 0) // 중간
                {
                    double dtHPrice = longTimeFrameMA.HighPrices[i];
                    double dtLPrice = longTimeFrameMA.LowPrices[i];
                    if (i != (longTimeFrameMA.HighPrices.Length - 1))
                    {
                        dtHPrice = (longTimeFrameMA.HighPrices[i + 1] - longTimeFrameMA.HighPrices[i]) / (dtBuff + 1);
                        dtLPrice = (longTimeFrameMA.LowPrices[i + 1] - longTimeFrameMA.LowPrices[i]) / (dtBuff + 1);
                    }
                    else //마지막
                    {
                        dtHPrice = 0;
                        dtLPrice = 0;
                    }
                    for (int j = 0; j <= dtBuff; j++)
                    {
                        shortTimeFrame[startBuff + j].ShowMovingAvrHigh = longTimeFrameMA.HighPrices[i] + (dtHPrice * j);
                        shortTimeFrame[startBuff + j].ShowMovingAvrLow = longTimeFrameMA.LowPrices[i] + (dtLPrice * j);
                    }
                }
                else
                {
                    Debug.WriteLine("하위 프레임에서는 해당 시간이 없음.");
                }
            }

            candleStickData = shortTimeFrame;
            //하단 채널
            savedCandleStickData.ReWriteCandleData(allCandleDatas);
        }

        private void LinearRegressionTest(ref CANDLE_STICK_DEF[] longTimeFrame)
        {
            CalcTrandLine tl = new CalcTrandLine();
            tl.Test3(ref longTimeFrame);
            candleStickData = longTimeFrame;
        }
        
        public LiveChartProperties GetCandleData()
        {
            //movingAverTest(ref allCandleDatas.DAY, ref allCandleDatas.MOVING_AVR.DAY, ref allCandleDatas.HOUR);
            LinearRegressionTest(ref allCandleDatas.WEEK);
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
                        Values = candleStickData.Select( x => x.ShowMovingAvrHigh).ToArray(),
                        Fill = null,
                        GeometrySize = 0,
                        Stroke = new SolidColorPaint(SKColors.Red)
                    },
                    new LineSeries<double>
                    {
                        Values = candleStickData.Select( x => x.ShowMovingAvrLow).ToArray(),
                        Fill = null,
                        GeometrySize = 0,
                        Stroke = new SolidColorPaint(SKColors.Blue)
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
