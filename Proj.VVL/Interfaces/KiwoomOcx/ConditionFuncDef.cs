using AxKHOpenAPILib;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    internal class ConditionFuncDef : IConditionHandle
    {
        public AxKHOpenAPI OcxObject;

        public ConditionFuncDef(AxKHOpenAPI OcxObjBind)
        {
            OcxObject = OcxObjBind;
        }

        /// <summary>
        /// 서버에 저장된 사용자 조건검색 목록을 요청합니다. 
        /// 조건검색 목록을 모두 수신하면 OnReceiveConditionVer()이벤트가 발생됩니다.
        /// 조건검색 목록 요청을 성공하면 1, 아니면 0을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public KIWOOM_STATE_DEF GetConditionLoad()
        {
            return (KIWOOM_STATE_DEF)OcxObject.GetConditionLoad();
        }

        /// <summary>
        /// 서버에서 수신한 사용자 조건식을 조건식의 고유번호와 조건식 이름을 한 쌍으로 하는 문자열들로 전달합니다.
        /// 조건식 하나는 조건식의 고유번호와 조건식 이름이 구분자 '^'로 나뉘어져 있으며 각 조건식은 ';'로 나뉘어져 있습니다.
        /// 이 함수는 OnReceiveConditionVer()이벤트에서 사용해야 합니다.
        /// </summary>
        /// <returns></returns>
        public string GetConditionNameList()
        {
            return OcxObject.GetConditionNameList();
        }

        /// <summary>
        /// 종목코드와 FID 리스트를 이용해서 실시간 시세를 등록하는 함수입니다.
        /// 한번에 등록가능한 종목과 FID갯수는 100종목, 100개 입니다.
        /// </summary>
        /// <param name="화면번호"></param>
        /// <param name="종목코드"></param>
        /// <param name="실시간FID리스트"></param>
        /// ;를 구분자로 사용한다.
        /// <param name="실시간등록타입"></param>
        /// 실시간 등록타입을 0으로 설정하면 등록한 종목들은 실시간 해지되고 등록한 종목만 실시간 시세가 등록됩니다.
        /// 실시간 등록타입을 1로 설정하면 먼저 등록한 종목들과 함께 실시간 시세가 등록됩니다
        /// <returns></returns>
        public ERROR_CODE_DEF SetRealReg(string 화면번호, string 종목코드, string 실시간FID리스트, string 실시간등록타입 = "1")
        {
            return (ERROR_CODE_DEF)OcxObject.SetRealReg(화면번호, 종목코드, 실시간FID리스트, 실시간등록타입);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="화면번호"></param>
        /// ALL <- 모든 실시간 종목 해지
        /// <param name="종목코드"></param>
        /// ALL <- 모든 실시간 종목 해지
        public void SetRealRemove(string 화면번호 = "ALL", string 종목코드 = "ALL")
        {
            OcxObject.SetRealRemove(화면번호, 종목코드);
        }
    }
}
