using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx.Abstractions
{
    internal interface IQueryHandle
    {
        public ERROR_CODE_DEF CommRqData(string 사용자구분명, string 조회하려는TR이름, int 연속조회여부, string 화면번호);
        public void SetInputValue(string TR에_명시된_Input이름, string Input이름으로_지정한_값);
        public void DisconnectRealData(string 화면번호);
        public int GetRepeatCnt(string TR이름, string 레코드이름);
        public ERROR_CODE_DEF CommKwRqData(string 조회종목리스트, int 종목코드개수, KIWOOM_nTypeFlag 타입, string 사용자구분명, string 화면번호);


    }
}
