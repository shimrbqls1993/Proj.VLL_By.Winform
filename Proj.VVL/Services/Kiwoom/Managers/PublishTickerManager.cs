using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Model.RecommandTickerModel;
using Proj.VVL.Interfaces.PubSub;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class PublishTickerManager : ServiceManagerBase
    {
        public ObservableCollection<Ticker> RecommandKoreaTickers = new ObservableCollection<Ticker>();
        public ObservableCollection<Ticker> RecommandUSTickers = new ObservableCollection<Ticker>();
        public ObservableCollection<Ticker> RecommandCryptoTickers = new ObservableCollection<Ticker>();

        public PublishTickerManager(IRecommandTickerHandler hRecommandTicker) 
        {
            RecommandKoreaTickers = hRecommandTicker.LoadData();
        }

        public void Start()
        {
            IsRunning = true;
            Handler = new Thread(Main);
            Handler.Start();
        }

        public void Stop() 
        {
            IsRunning = false;
            Handler.Join(2000);
        }

        public void Main()
        {

        }
    }
}
