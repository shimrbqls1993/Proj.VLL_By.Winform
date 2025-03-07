using AxKHOpenAPILib;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    internal class QueryFuncDef : IQueryHandle
    {
        public AxKHOpenAPI OcxObject;

        public QueryFuncDef(AxKHOpenAPI OcxObjBind)
        {
            OcxObject = OcxObjBind;
        }

        public ERROR_CODE_DEF CommRqData(string 사용자구분명, string 조회하려는TR이름, int 연속조회여부, string 화면번호)
        {
            return (ERROR_CODE_DEF)OcxObject.CommRqData(사용자구분명, 조회하려는TR이름, 연속조회여부, 화면번호);
        }

        /// <summary>
        /// 조회요청 시 TR의 Input값을 지정하는 함수입니다.
        /// CommRqData 호출 전에 입력값들을 세팅합니다.
        /// 각 TR마다 Input 항목이 다릅니다. 순서에 맞게 Input값들을 세팅해야 합니다.
        /// </summary>
        public void SetInputValue(string TR에_명시된_Input이름, string Input이름으로_지정한_값)
        {
            OcxObject.SetInputValue(TR에_명시된_Input이름, Input이름으로_지정한_값);
        }

        /// <summary>
        /// 시세데이터를 요청할때 사용된 화면번호를 이용하여
        /// 해당 화면번호로 등록되어져 있는 종목의 실시간시세를 서버에 등록해지 요청합니다.
        /// 이후 해당 종목의 실시간시세는 수신되지 않습니다.
        /// 단, 해당 종목이 또다른 화면번호로 실시간 등록되어 있는 경우 해당종목에대한 실시간시세 데이터는 계속 수신됩니다.
        /// </summary>
        /// <param name="화면번호"></param>
        public void DisconnectRealData(string 화면번호)
        {
            OcxObject.DisconnectRealData(화면번호);
        }


        /// <summary>
        /// 데이터 반복 건수 구함
        /// 이 함수는 OnReceivceTRData()이벤트가 발생될 때 그 안에서 사용해야합니다.
        /// </summary>
        /// <param name="TR이름"></param>
        /// <param name="레코드이름"></param>
        /// <returns></returns>
        public int GetRepeatCnt(string TR이름, string 레코드이름)
        {
            return OcxObject.GetRepeatCnt(TR이름, 레코드이름);
        }

        /// <summary>
        /// 한번에 100종목까지 조회할 수 있는 복수종목 조회함수 입니다.
        /// 함수인자로 사용하는 종목코드 리스트는 조회하려는 종목코드 사이에 구분자';'를 추가해서 만들면 됩니다.
        /// 수신되는 데이터는 TR목록에서 복수종목정보요청(OPTKWFID) Output을 참고하시면 됩니다.
        /// ※ OPTKWFID TR은 CommKwRqData() 함수 전용으로, CommRqData 로는 사용할 수 없습니다.
        /// ※ OPTKWFID TR은 영웅문4 HTS의 관심종목과는 무관합니다. 
        /// </summary>
        /// <param name="조회종목리스트"></param>
        /// <param name="종목코드개수"></param>
        /// <param name="타입"></param>
        /// <param name="사용자구문명"></param>
        /// <param name="화면번호"></param>
        /// <returns></returns>
        public ERROR_CODE_DEF CommKwRqData(string 조회종목리스트, int 종목코드개수, KIWOOM_nTypeFlag 타입, string 사용자구분명, string 화면번호)
        {
            return (ERROR_CODE_DEF)OcxObject.CommKwRqData(조회종목리스트, 0, 종목코드개수, (int)타입, 사용자구분명, 화면번호);
        }
    }
}
