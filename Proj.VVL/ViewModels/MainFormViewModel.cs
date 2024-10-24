using CommunityToolkit.Mvvm.Input;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using Proj.VVL.Model;
using Proj.VVL.Services.Abstractions;
using Proj.VVL.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proj.VVL.ViewModels
{
    public class MainFormViewModel : ViewModelBase
    {
        public MainFormViewModel(ILoginHandler login)
        {
            Text = "LOGIN STATUS";
            LoginCommand = new RelayCommand(login.Login);

        }

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        private UiOption.UI_STATUS _STATUS;
        public UiOption.UI_STATUS LOGIN_STATUS
        {
            get { return _STATUS; }
            set
            {
                _STATUS = value;
                BackColor = UiOption.UI_STATUS_COLOR_DEF[value].Back;
                ForeColor = UiOption.UI_STATUS_COLOR_DEF[value].Fore;
            }
        }
        
        Color _BackColor = Color.White;
        public Color BackColor
        {
            get { return _BackColor; }
            set
            {
                SetProperty(ref _BackColor, value);
            }
        }

        Color _ForeColor = Color.Black;
        public Color ForeColor
        {
            get { return _ForeColor; }
            set
            {
                SetProperty(ref _ForeColor, value);
            }
        }
    }
}
