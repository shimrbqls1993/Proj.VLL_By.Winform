using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Painting;
using Proj.VVL.Interfaces.DataInventoryHandlers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Model.RecommandTickerModel;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Proj.VVL.Data
{
    public class Define
    {
        public static string LOG_DIR_PATH = Application.StartupPath + "\\Logger";
        public static string COLLECTION_DIR_PATH = Application.StartupPath + "\\Collection";

        public static string DATA_DIR_PATH = Application.StartupPath + "\\Data";
        public static string CANDLE_STICK_DIR_PATH = DATA_DIR_PATH + "\\CandleSticks";

        public static string RECOMMAND_TICKER_DATA_PATH = DATA_DIR_PATH + "\\Recommand_Ticker.xlsx";
        public const string SHEET_NAME_CRYPTO = "Crypto";
        public const string SHEET_NAME_K_STOCK = "Korea_Stock";
        public const string SHEET_NAME_US_STOCK = "US_STOCK";
        public const string ShortStr2DateTimeFormat = "yyyyMMdd";
        public const string LongStr2DateTimeFormat = "yyyyMMddHHmmss";

        public static bool CreateAllDirectory()
        {
            try
            {
                if (!Directory.Exists(LOG_DIR_PATH))
                {
                    Directory.CreateDirectory(LOG_DIR_PATH);
                }
                if (!Directory.Exists(COLLECTION_DIR_PATH))
                {
                    Directory.CreateDirectory(COLLECTION_DIR_PATH);
                }
                if (!Directory.Exists(DATA_DIR_PATH))
                {
                    Directory.CreateDirectory(DATA_DIR_PATH);
                }
                if (!Directory.Exists(CANDLE_STICK_DIR_PATH))
                {
                    Directory.CreateDirectory(CANDLE_STICK_DIR_PATH);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }

    public class KIWOOM
    {
        public static ObservableCollection<Ticker> KOREA_TICKERS = new ObservableCollection<Ticker>();
    }

    public class LiveChartProperties
    {
        public IEnumerable<ISeries> Series { get; set; }
        public IEnumerable<ICartesianAxis> xAxes { get; set; }
        public IEnumerable<ICartesianAxis> yAxes { get; set; }
    }

    public class CANDLE_DATA_DEF
    {
        public CANDLE_STICK_DEF[] WEEK = Array.Empty<CANDLE_STICK_DEF>();
        public CANDLE_STICK_DEF[] DAY = Array.Empty<CANDLE_STICK_DEF>();
        public CANDLE_STICK_DEF[] HOUR = Array.Empty<CANDLE_STICK_DEF>();
        public CANDLE_STICK_DEF[] MIN_5 = Array.Empty<CANDLE_STICK_DEF>();
        public CANDLE_STICK_DEF[] MIN_1 = Array.Empty<CANDLE_STICK_DEF>();
        public SUPPORT_REGIST_LEVEL_DEF[] SUPPORT_REGIST_LINE = Array.Empty<SUPPORT_REGIST_LEVEL_DEF>();
        public MOVING_AVR_DEF MOVING_AVR = new MOVING_AVR_DEF();
        public FIBONACCI[] F_PATTERN = Array.Empty<FIBONACCI>();
        public ELLIOTT_WAVE_IMPULSE[] IMPULSE = Array.Empty<ELLIOTT_WAVE_IMPULSE>();
        public ELLIOTT_WAVE_CORRECTION[] CORRECTION = Array.Empty <ELLIOTT_WAVE_CORRECTION>();
    }

    public class SUPPORT_REGIST_LEVEL_DEF
    {
        public double price;
        public CANDLE_TIME_FRAME_DEF time;
    }

    public class MOVING_AVR_DEF
    {
        public MOVING_AVR_PROPERTIES HOUR = new MOVING_AVR_PROPERTIES();
        public MOVING_AVR_PROPERTIES DAY = new MOVING_AVR_PROPERTIES();
        public MOVING_AVR_PROPERTIES WEEK = new MOVING_AVR_PROPERTIES();
    }

    public class MOVING_AVR_PROPERTIES
    {
        public double[] HighPrices = Array.Empty<double>();
        public double[] LowPrices = Array.Empty<double>();
        public double[] OpenPrices = Array.Empty<double>();
        public double[] ClosePrices = Array.Empty<double>();
        public string[] DateTime = Array.Empty<string>();
    }

    public class ELLIOTT_WAVE_CORRECTION
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
    }

    public class ELLIOTT_WAVE_IMPULSE
    {
        public int W1 { get; set; }
        public int W2 { get; set; }
        public int W3 { get; set; }
        public int W4 { get; set; }
        public int W5 { get; set; }
    }

    public class FIBONACCI
    {
        public int LINE_0 { get; set; }
        public int LINE_236 { get; set; }
        public int LINE_382 { get; set; }
        public int LINE_5 { get; set; }
        public int LINE_618 { get; set; }
        public int LINE_786 { get; set; }
        public int LINE_1 { get; set; }
        public int LINE_1414 { get; set; }
        public int LINE_1618 { get; set; }
    }

    public class CANDLE_STICK_DEF
    {
        public string Datetime { get; set; }
        public double Close { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public int dtDateTime { get; set; }
        public double ShowMovingAvrClose { get; set; }
        public double ShowMovingAvrOpen { get; set; }
        public double ShowMovingAvrHigh { get; set; }
        public double ShowMovingAvrLow { get; set; }
    }

    public class CommonFunc()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parseStr"></param>
        /// <param name="dateTimeFormat"></param>
        /// Day Time frame 이상이면 true 아니면 false
        /// <param name="result"></param>
        /// <returns></returns>
        public static DateTime ParseString2DateTime(string parseStr)
        {
            if(DateTime.TryParseExact(parseStr, Define.ShortStr2DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return DateTime.ParseExact(parseStr, Define.LongStr2DateTimeFormat, CultureInfo.InvariantCulture);
            }
        }

        public static double CalcPercent(double value, double percent)
        {
            if (percent == 0)
                return percent;
            return (value * (percent / 100));
        }

        public static int CalcMovingAvr(double[] 종가s)
        {
            double result = 0;

            foreach (double 종가 in 종가s)
            {
                result += 종가;
            }
            result /= 종가s.Length;

            return (int)result;
        }

        public static bool CandleDataIsVaild(CANDLE_DATA_DEF candleData)
        {
            Type candlePricesType = typeof(CANDLE_STICK_DEF);
            Type candleDataType = typeof(CANDLE_DATA_DEF);

            FieldInfo[] pricesFields = candleDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                foreach (FieldInfo priceField in pricesFields)
                {
                    if (candlePricesType == priceField)
                    {
                        object priceData = priceField.GetValue(candleData);
                        if (priceData == null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
