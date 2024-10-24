using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx.Abstractions
{
    internal interface IOrderHandle
    {
        public ERROR_CODE_DEF SendOrder(string 사용자구분명, string 화면번호, string 계좌번호, KIWOOM_nOrderType type, string 종목코드, int 주문수량, int 주문가격, KIWOOM_sHogaGb 거래구분, string 주문번호);
        public ERROR_CODE_DEF SendOrderFO(string 사용자구분명, string 화면번호, string 계좌번호, string 종목코드, KIWOOM_lOrdKind 주문종류, KIWOOM_sSlbyTp 매매구분, KIWOOM_sOrdTp 거래구분, int 주문수량, string 주문가격, string 원주문번호);
        public ERROR_CODE_DEF SendOrderCredit(string 사용자구분명, string 화면번호, string 계좌번호, KIWOOM_nOrderType 주문유형, string 종목코드, int 주문수량, int 주문가격, KIWOOM_sHogaGb 거래구분, KIWOOM_sCreditGb 신용거래구분, string 원주문번호);
        public string GetChejanData(int fid);
    }
}
