using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Model.RecommandTickerModel;

namespace Proj.VVL.Data
{
    public class Define
    {
        public static string LOG_DIR_PATH = Application.StartupPath + "\\Logger";
        public static string COLLECTION_DIR_PATH = Application.StartupPath + "\\Collection";
        public static string DATA_DIR_PATH = Application.StartupPath + "\\Data";
        public static string RECOMMAND_TICKER_DATA_PATH = DATA_DIR_PATH + "\\Recommand_Ticker.xlsx";
        public const string SHEET_NAME_CRYPTO = "Crypto";
        public const string SHEET_NAME_K_STOCK = "Korea_Stock";
        public const string SHEET_NAME_US_STOCK = "US_STOCK";

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
}
