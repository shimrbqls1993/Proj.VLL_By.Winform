using AxKHOpenAPILib;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    /// <summary>
    /// 스레드를 따로 만들어서 관리해야할 듯
    /// </summary>
    internal class OrderFuncDef : IOrderHandle
    {
        public AxKHOpenAPI OcxObject;

        public OrderFuncDef(AxKHOpenAPI OcxObjBind)
        {
            OcxObject = OcxObjBind;
        }

        /// <summary>
        /// 서버에 주문을 전송하는 함수 입니다.
        /// 1초에 5회만 주문가능하며 그 이상 주문요청하면 에러 -308을 리턴합니다.
        /// 시장가주문시 주문가격은 0으로 입력합니다.
        /// 취소주문일때 주문가격은 0으로 입력합니다.
        /// </summary>
        /// <param name="사용자구분명"></param>
        /// <param name="화면번호"></param>
        /// 서버에 데이터를 요청하거나, 주문을 발생시킬때 사용합니다.
        /// 화면번호는 서버의 결과를 수신할 때 어떤 요청에 의한 수신인지를 구별하기 위한 키값의 개념입니다.
        /// 0000을 제외한 임의의 숫자를 자유롭게 사용하면 됨.
        /// 같은 화면 번호로 데이터 요청을 빠르게 반복하는 경우 데이터의 유효성을 보장할 수 없음
        /// 최소 2개의 화면번호를 번갈아가며 또는 매번 새로운 화면번호 사용을 권장
        /// 사용자 프로그램에서 사용할 수 있는 화면번호 갯수가 200개로 한장되어 있음.
        /// 화면번호 갯수가 200개가 넘어야하는 경우 이전에 사용되었던 화면번호를 재사용하는 방식
        /// <param name="계좌번호"></param>
        /// <param name="type"></param>
        /// <param name="종목코드"></param>
        /// <param name="주문수량"></param>
        /// <param name="주문가격"></param>
        /// <param name="거래구분"></param>
        /// <param name="주문번호"></param>
        /// <returns></returns>
        public ERROR_CODE_DEF SendOrder(string 사용자구분명, string 화면번호, string 계좌번호, KIWOOM_nOrderType type, string 종목코드, int 주문수량, int 주문가격, KIWOOM_sHogaGb 거래구분, string 주문번호)
        {
            string temp거래구분 = ((int)거래구분).ToString("D2");
            return (ERROR_CODE_DEF)OcxObject.SendOrder(사용자구분명, 화면번호, 계좌번호, (int)type, 종목코드, 주문수량, 주문가격, temp거래구분, 주문번호);
        }

        /// <summary>
        /// 서버에 주문을 전송하는 함수 입니다.
        /// 코스피지수200 선물옵션, 주식선물 전용 주문함수입니다.
        /// </summary>
        /// <param name="사용자구분명"></param>
        /// <param name="화면번호"></param>
        /// <param name="계좌번호"></param>
        /// <param name="종목코드"></param>
        /// <param name="주문종류"></param>
        /// <param name="매매구분"></param>
        /// <param name="거래구분"></param>
        /// <param name="주문수량"></param>
        /// <param name="주문가격"></param>
        /// <param name="원주문번호"></param>
        /// <returns></returns>
        public ERROR_CODE_DEF SendOrderFO(string 사용자구분명, string 화면번호, string 계좌번호, string 종목코드, KIWOOM_lOrdKind 주문종류, KIWOOM_sSlbyTp 매매구분, KIWOOM_sOrdTp 거래구분, int 주문수량, string 주문가격, string 원주문번호)
        {
            string temp매매구분 = ((int)매매구분).ToString();
            string temp거래구분 = ((int)거래구분).ToString();
            if (거래구분 == KIWOOM_sOrdTp.최유리지정가FOK)
            {
                temp거래구분 = "A";
            }
            return (ERROR_CODE_DEF)OcxObject.SendOrderFO(사용자구분명, 화면번호, 계좌번호, 종목코드, (int)주문종류, temp매매구분, temp거래구분, 주문수량, 주문가격, 원주문번호);
        }

        /// <summary>
        /// 서버에 주문을 전송하는 함수 입니다.
        /// 국내주식 신용주문 전용함수입니다. 대주거래는 지원하지 않습니다.
        /// </summary>
        /// <returns></returns>
        public ERROR_CODE_DEF SendOrderCredit(string 사용자구분명, string 화면번호, string 계좌번호, KIWOOM_nOrderType 주문유형, string 종목코드, int 주문수량, int 주문가격, KIWOOM_sHogaGb 거래구분, KIWOOM_sCreditGb 신용거래구분, string 원주문번호)
        {
            string temp거래구분 = ((int)거래구분).ToString("D2");
            string temp신용거래구분 = ((int)신용거래구분).ToString("D2");
            string 대출일 = DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "");
            return (ERROR_CODE_DEF)OcxObject.SendOrderCredit(사용자구분명, 화면번호, 계좌번호, (int)주문유형, 종목코드, 주문수량, 주문가격, temp거래구분, temp신용거래구분, 대출일, 원주문번호);
        }

        /// <summary>
        /// OnReceiveChejan()이벤트가 발생될때 FID에 해당되는 값을 구하는 함수입니다.
        /// 이 함수는 OnReceiveChejan() 이벤트 안에서 사용해야 합니다.
        /// 예) 체결가 = GetChejanData(910) 
        /// </summary>
        /// <param name="FID"></param>
        /// 실시간 타입에 포함된 Field ID
        /// <returns></returns>
        public string GetChejanData(int fid)
        {
            return OcxObject.GetChejanData(fid);
        }
    }
}
