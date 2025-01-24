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

        public LiveChartProperties GetCandleData()
        {
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
