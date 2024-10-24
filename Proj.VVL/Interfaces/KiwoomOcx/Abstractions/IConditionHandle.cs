using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx.Abstractions
{
    internal interface IConditionHandle
    {
        public KIWOOM_STATE_DEF GetConditionLoad();
        public string GetConditionNameList();
        public ERROR_CODE_DEF SetRealReg(string 화면번호, string 종목코드, string 실시간FID리스트, string 실시간등록타입 = "1");
        public void SetRealRemove(string 화면번호 = "ALL", string 종목코드 = "ALL");
    }
}
