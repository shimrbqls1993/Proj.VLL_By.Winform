using Proj.VVL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Interfaces.KiwoomHandlers.RecommandTickerHandler;

namespace Proj.VVL.Interfaces.KiwoomHandlers.Abstractions
{
    public interface IRecommandTickerHandler
    {
        public ObservableCollection<RecommandTickerModel.Ticker> LoadData();
        public bool SaveData(int index, string name, string code, ObservableCollection<RecommandTickerModel.Ticker> tickers);
        public bool DeleteData(int index, string name, string code, ObservableCollection<RecommandTickerModel.Ticker> tickers);
    }
}
