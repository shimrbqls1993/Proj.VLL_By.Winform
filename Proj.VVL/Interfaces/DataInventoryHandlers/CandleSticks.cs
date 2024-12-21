using Proj.VVL.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Model.RecommandTickerModel;

namespace Proj.VVL.Interfaces.DataInventoryHandlers
{
    public class CandleSticks
    {
        public string Code = string.Empty;
        public string FileName = string.Empty;
        public string FilePath = string.Empty;
        JsonHandler jHandle = new JsonHandler();
        public CANDLE_DATA_DEF Datas;

        public CandleSticks(string code)
        {
            Code = code;
            FileName = $"{code}.json";
            FilePath = Path.Combine(Data.Define.CANDLE_STICK_DIR_PATH, FileName);
            IsCandleStickFileExist();
            Datas = GetCandleDatasFromFile();
        }

        public CANDLE_DATA_DEF GetCandleDatasFromFile()
        {
            if (File.Exists(FilePath))
            {
                return jHandle.ReadJsonFromFile<CANDLE_DATA_DEF>(FilePath);
            }
            else
            {
                return new CANDLE_DATA_DEF();
            }
        }

        public void IsCandleStickFileExist()
        {
            string path = Path.Combine(FilePath);
            if (!File.Exists(FilePath))
            {
                Datas = new CANDLE_DATA_DEF();
                jHandle.WriteJsonToFile(Datas, FilePath);
            }
        }

        public void AddChartCandleStick(CANDLE_STICK_DEF[] newCandleDatas, CANDLE_TIME_FRAME_DEF timeFrame)
        {
            int newCandleDataIndex = 0;
            List<CANDLE_STICK_DEF>temp = SelectData(timeFrame).ToList();
            for(newCandleDataIndex = 0; newCandleDataIndex<newCandleDatas.Length; newCandleDataIndex++)
            {
                if (temp.IndexOf(newCandleDatas[newCandleDataIndex]) == -1)
                {
                    if (temp.Count > 5000)
                    {
                        temp.RemoveAt(temp.Count - 1);
                    }
                    temp.Add(newCandleDatas[newCandleDataIndex]);
                }
            }

            switch (timeFrame)
            {
                case CANDLE_TIME_FRAME_DEF.WEEK:
                    Datas.WEEK = temp.ToArray();
                    break;
                case CANDLE_TIME_FRAME_DEF.DAY:
                    Datas.DAY = temp.ToArray(); ;
                    break;
                case CANDLE_TIME_FRAME_DEF.HOUR:
                    Datas.HOUR = temp.ToArray();;
                    break;
                case CANDLE_TIME_FRAME_DEF.MIN_5:
                    Datas.MIN_5 = temp.ToArray();;
                    break;
                case CANDLE_TIME_FRAME_DEF.MIN_1:
                    Datas.MIN_1 = temp.ToArray();;
                    break;
            }
            jHandle.WriteJsonToFile(Datas, FilePath);
        }

        private CANDLE_STICK_DEF[] SelectData(CANDLE_TIME_FRAME_DEF timeFrame)
        {
            switch (timeFrame)
            {
                case CANDLE_TIME_FRAME_DEF.WEEK:
                    return Datas.WEEK;
                case CANDLE_TIME_FRAME_DEF.DAY:
                    return Datas.DAY;
                case CANDLE_TIME_FRAME_DEF.HOUR:
                    return Datas.HOUR;
                case CANDLE_TIME_FRAME_DEF.MIN_5:
                    return Datas.MIN_5;
                case CANDLE_TIME_FRAME_DEF.MIN_1:
                    return Datas.MIN_1;
                default:
                    return Array.Empty<CANDLE_STICK_DEF>();
            }
        }
    }

    public enum CANDLE_TIME_FRAME_DEF
    {
        WEEK,
        DAY,
        HOUR,
        MIN_5,
        MIN_1
    }
}
