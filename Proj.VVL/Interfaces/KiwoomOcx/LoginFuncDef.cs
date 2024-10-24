using AxKHOpenAPILib;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    internal class LoginFuncDef : ILoginHandle
    {
        public AxKHOpenAPI OcxObject;
        public ERROR_CODE_DEF LOGIN_ERROR_CODE;

        /// <summary>
        /// 로그인이 완료되면 프로그램 시작
        /// 수동재로그인 할 수 있는 기능 제작
        /// 자동재로그인이 필요 한지는 불명
        /// </summary>
        /// <param name="OcxObjBind"></param>
        public LoginFuncDef(AxKHOpenAPI OcxObjBind)
        {
            OcxObject = OcxObjBind;
            OcxObject.OnEventConnect += OnEventConnect;
        }

        public ERROR_CODE_DEF CommConnect()
        {
            return (ERROR_CODE_DEF)OcxObject.CommConnect();
        }

        public KIWOOM_STATE_DEF GetConnectState()
        {
            return (KIWOOM_STATE_DEF)OcxObject.GetConnectState();
        }

        public string GetLoginInfo(GetLoginInfo_Param_DEF param)
        {
            return OcxObject.GetLoginInfo(Define.GetLoginInfo_Param[param]);
        }

        public void OnEventConnect(object sendor, _DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            Console.WriteLine($"로그인 결과 : {e.nErrCode}");
            LOGIN_ERROR_CODE = (ERROR_CODE_DEF)e.nErrCode;
        }
    }
}
