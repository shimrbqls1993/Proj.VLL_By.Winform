using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx.Abstractions
{
    public interface ILoginHandle
    {
        public ERROR_CODE_DEF CommConnect();
        public KIWOOM_STATE_DEF GetConnectState();
        public string GetLoginInfo(GetLoginInfo_Param_DEF param);
    }
}
