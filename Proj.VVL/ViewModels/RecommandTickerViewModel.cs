using CommunityToolkit.Mvvm.Input;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Proj.VVL.Model.RecommandTickerModel;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Proj.VVL.ViewModels
{
    public class RecommandTickerViewModel : ViewModelBase
    {
        IRecommandTickerHandler _handler;
        public RecommandTickerViewModel(IRecommandTickerHandler recommandTicker)
        {
            _handler = recommandTicker;
            SaveBtnClick = new RelayCommand(SaveClick);
            DeleteBtnClick = new RelayCommand(DeleteClick);
        }

        public ICommand SaveBtnClick { get; set; }
        public ICommand DeleteBtnClick { get; set; }

        private void SaveClick()
        {
            if(_handler.SaveData(MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers.Count + 2, Name, Code, MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers))
            {
                MessageBox.Show("저장 성공");
            }
            else
            {
                MessageBox.Show("저장 실패");
            }
        }

        private void DeleteClick()
        {
            try
            {
                if(SelectRow == -1)
                {
                    MessageBox.Show("선택한 데이터가 없습니다.");
                    return;
                }
                if (_handler.DeleteData(SelectRow + 2, SelectName, SelectCode, MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers))
                {
                    MessageBox.Show("삭제 성공");
                }
                else
                {
                    MessageBox.Show("삭제 실패");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _code = string.Empty;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        public int SelectRow { get; set; } = -1;
        public string SelectName { get; set; }
        public string SelectCode { get; set; }
    }
}
