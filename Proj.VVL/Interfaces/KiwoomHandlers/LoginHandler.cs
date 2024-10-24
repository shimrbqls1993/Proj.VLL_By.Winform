using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomHandlers
{
    internal class LoginHandler : ILoginHandler
    {
        public void Login()
        {
            if (MainForm.KiwoomOcxObj.login.CommConnect() != (int)ERROR_CODE_DEF.NONE)
            {
                MessageBox.Show("로그인 실패");
            }
        }
    }
}
