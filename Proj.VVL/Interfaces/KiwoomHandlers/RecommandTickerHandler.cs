using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Model;

namespace Proj.VVL.Interfaces.KiwoomHandlers
{
    public class RecommandTickerHandler : RecommandTickerModel, IRecommandTickerHandler
    {
        public RecommandTickerHandler()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        enum RECOMMAND_TICKER_COLUMN_DEF
        {
            NAME = 1,
            CODE,
        }

        public ObservableCollection<Ticker> LoadData()
        {
            try
            {
                ObservableCollection<Ticker> tickers = new ObservableCollection<Ticker>();

                string filePath = Data.Define.RECOMMAND_TICKER_DATA_PATH;
                if (!File.Exists(filePath))
                {
                    using (ExcelPackage pack = new ExcelPackage())
                    {
                        pack.Workbook.Worksheets.Add(Data.Define.SHEET_NAME_K_STOCK);

                        pack.Workbook.Worksheets[0].Cells[1, (int)RECOMMAND_TICKER_COLUMN_DEF.NAME].Value = "Name";
                        pack.Workbook.Worksheets[0].Cells[1, (int)RECOMMAND_TICKER_COLUMN_DEF.CODE].Value = "Code";

                        pack.Workbook.Worksheets.Add(Data.Define.SHEET_NAME_US_STOCK);
                        pack.Workbook.Worksheets.Add(Data.Define.SHEET_NAME_CRYPTO);

                        FileInfo fi = new FileInfo(filePath);
                        pack.SaveAs(fi);
                    }
                    return null;
                }
                else
                {
                    FileInfo info = new FileInfo(filePath);
                    using (ExcelPackage pack = new ExcelPackage(info))
                    {
                        pack.Workbook.Worksheets.MoveToStart(Data.Define.SHEET_NAME_K_STOCK);
                        ExcelWorksheet kStockws = pack.Workbook.Worksheets[0];

                        if (kStockws.Dimension == null)
                        {
                            return null;
                        }

                        for (int row = 2; row <= kStockws.Dimension.End.Row; row++)
                        {
                            Ticker temp = new Ticker(
                            kStockws.Cells[row, (int)RECOMMAND_TICKER_COLUMN_DEF.NAME].Text.Trim(),
                            kStockws.Cells[row, (int)RECOMMAND_TICKER_COLUMN_DEF.CODE].Text.Trim());
                            tickers.Add(temp);
                        }
                    }
                }
                return tickers;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool SaveData(int index, string name, string code, ObservableCollection<Ticker>tickers)
        {
            try
            {
                if(IsAlreadyPulished(code, tickers))
                {
                    return false;
                }
                FileInfo info = new FileInfo(Data.Define.RECOMMAND_TICKER_DATA_PATH);
                using (ExcelPackage pack = new ExcelPackage(info))
                {
                    pack.Workbook.Worksheets.MoveToStart(Data.Define.SHEET_NAME_K_STOCK);
                    pack.Workbook.Worksheets[0].Cells[index, (int)RECOMMAND_TICKER_COLUMN_DEF.NAME].Value = name;
                    pack.Workbook.Worksheets[0].Cells[index, (int)RECOMMAND_TICKER_COLUMN_DEF.CODE].Value = code;
                    pack.Save();
                }
                tickers.Add(new Ticker(name, code));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteData(int index, string name, string code, ObservableCollection<Ticker> tickers)
        {
            try
            {
                FileInfo info = new FileInfo(Data.Define.RECOMMAND_TICKER_DATA_PATH);
                using (ExcelPackage pack = new ExcelPackage(info))
                {
                    pack.Workbook.Worksheets.MoveToStart(Data.Define.SHEET_NAME_K_STOCK);
                    pack.Workbook.Worksheets[0].DeleteRow(index);
                    pack.Save();
                }

                foreach (Ticker ticker in tickers)
                {
                    if (ticker.Name == name && ticker.Code == code)
                    {
                        tickers.Remove(ticker);
                        Debug.WriteLine("remove next seq");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private bool IsAlreadyPulished(string code, ObservableCollection<Ticker> tickersCollection)
        {
            foreach (Ticker ticker in tickersCollection)
            {
                if (!string.IsNullOrEmpty(ticker.Code))
                {
                    if(ticker.Code == code)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
