using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using Proj.VVL.Model;
using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proj.VVL.Services.Kiwoom.Managers
{
    public class LoginManager
    {
        System.Threading.Timer timer;
        AutoResetEvent autoResetEvent;

        public void Start()
        {
            autoResetEvent = new AutoResetEvent(false);
            timer = new System.Threading.Timer(CheckLoginState, autoResetEvent, 0, 5000);
        }

        public void Stop()
        {
            autoResetEvent.WaitOne();
            timer.Dispose();
        }

        public void CheckLoginState(object? sender)
        {
            try
            {
                 if (MainForm.KiwoomOcxObj.login.GetConnectState() == KIWOOM_STATE_DEF.OK)
                {
                    MainForm.Instance.viewModel.LOGIN_STATUS = UiOption.UI_STATUS.OK;
                }
                else
                {
                    MainForm.Instance.viewModel.LOGIN_STATUS = UiOption.UI_STATUS.FAIL;
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
