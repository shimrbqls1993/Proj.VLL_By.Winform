using Proj.VVL.Interfaces.DataInventoryHandlers;
using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.PubSub
{
    /// <summary>
    /// 아직 이 부분에 대한 필요성을 못 느낌
    /// PublishTickerManager로 변경
    /// </summary>
    public class Publisher : RecommandTickerModel
    {
        public ObservableCollection<Published> publishedTickers = new ObservableCollection<Published>();
        private ObservableCollection<Ticker> _tickers;
        public Publisher(ObservableCollection<Ticker> tickers)
        {
            _tickers = tickers;
            //tickers.CollectionChanged += TickersChanged;
        }

        public void Init()
        {
            for (int i = 0; i < _tickers.Count; i++)
            {
                if (!string.IsNullOrEmpty(_tickers[i].Code))
                {
                    CreateSubscribers(_tickers[i].Code);
                }
            }
        }

        void CreateSubscribers(string code)
        {
            
        }

        void ReactiveSubscribers(string code)
        {

        }

        void DisableSubscribers(string code)
        {

        }

        void RemoveSubscribers(string code)
        {

        }

        void TickersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("Is ticker changed occur");
            ObservableCollection<Ticker> tickers = (ObservableCollection<Ticker>)sender;
            foreach (Ticker ticker in tickers)
            {
                Debug.WriteLine("Is checking ticker");
                if (!string.IsNullOrEmpty(ticker.Code))
                {
                    if (!IsAlreadyPublished(ticker.Code))
                    {
                        IsNewPublished(ticker.Code);
                    }
                }
            }

            foreach (Published alreadyPublished in publishedTickers)
            {
                if (!string.IsNullOrEmpty(alreadyPublished.Code))
                {
                    if (!CheckRecommandPublished(alreadyPublished, tickers))
                    {
                        IsDisabledPublished(alreadyPublished);
                        return;
                    }
                }
            }
        }

        bool IsAlreadyPublished(string checkCode)
        {
            foreach (Published alreadyPublished in publishedTickers)
            {
                if (!string.IsNullOrEmpty(alreadyPublished.Code))
                {
                    if (alreadyPublished.Code == checkCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void IsNewPublished(string checkCode)
        {
            publishedTickers.Add(new Published(checkCode));
            CreateSubscribers(checkCode);
        }

        bool CheckRecommandPublished(Published alreadyPublished, ObservableCollection<Ticker> tickers)
        {
            for (int i = 0; i < tickers.Count; i++)
            {
                if (tickers[i].Code == alreadyPublished.Code)
                {
                    return true;
                }
            }
            return false;
        }

        void IsDisabledPublished(Published removeTicker)
        {
            if (publishedTickers.Remove(removeTicker))
            {
                DisableSubscribers(removeTicker.Code);
            }
            else
            {
                MessageBox.Show("Published Ticker Removed Failed");
            }
        }

        public class Published(string code, bool isActive = true)
        {
            public string Code { get; set; } = code;
            public bool IsActive { get; set; } = isActive;
        }
    }
}
