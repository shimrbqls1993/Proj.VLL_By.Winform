using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    public enum KIWOOM_OPT_TR_CODE_DEF
    {
        주식기본정보요청 = 10001,
        주식거래원요청,
        체결정보요청,
        주식호가요청,
        주식일주월시분요청,
        주식시분요청,
        시세표성정보요청,
        주식외국인요청,
        주식기관요청,
        업종프로그램요청,
        신주인수권전체시세요청,
        주문체결요청,
        신용매매동향요청,
        공매도추이요청,
        일별거래상세요청,
        신고저가요청,
        상하한가요청,
        고저가근접요청,
        가격급등락요청,
        호가잔량상위요청,
        잔량율급증요청,
        거래량급증요청,
        거래량갱신요청,
        매물대집중요청,
        고저PER요청,
        전일대비등락률상위요청,
        시가대비등락률요청,
        예상체결등락률상위요청,
        당일거래량상위요청,
        전일거래량상위요청,
        거래대금상위요청,
        신용비율상위요청,
        외인기간별매매상위요청,
        외인연속순매매상위요청,
        외인한도소진율증가상위,
        외국계창구매매상위요청,
        종목별증권사순위요청,
        증권사별매매상위요청,
        조기종료통화단위요청,
        순매수거래원순위요청,
        거래원매물대분석요청,
        일별기관매매종목요청,
        종목별기관매매추이요청,
        체결강도추이시간별요청,
        체결강도추이일별요청,
        ELW일별민감도지표요청,
        ELW투자지표요청,
        ELW민감도지표요청,
        업종별투자자순매수요청,
        거래원순간거래량요청,
        당일상위이탈원요청,
        변동성완화장치발동종목요청,
        당일전일체결대량요청,
        투자자별일별매매종목요청,
        종목별투자자기관별요청,
        종목별투자자기관별차트요청,
        종목별투자자기관별합계요청,
        동일순매매순위요청,
        장중투자자별매매요청,
        장중투자자별매매차트요청,
        장중투자자별매매상위요청,
        대차거래내역요청,
        대차거래추이요청,
        대차거래상위10종목요청,
        당일주요거래원요청,
        시간대별전일비거래비중요청,
        일자별종목별실현손익요청 = 10073,
        일자별실현손익요청,
        미체결요청,
        체결요청,
        당일실현손익상세요청,
        증권사별종목매매동향요청,
        주식틱차트조회요청,
        주식분봉차트조회요청,
        주식일봉차트조회요청,
        주식주봉차트조회요청,
        주식월봉차트조회요청,
        당일전일체결요청,
        계좌수익률요청,
        일별주가요청,
        시간외단일가요청,
        주식년봉차트조회요청 = 10094,
        시간외단일가등락율순위요청 = 10098,
        기관외국인연속매매현황요청 = 10131,
        당일매매일지요청 = 10170,
        업종현재가요청 = 20001,
        업종별주가요청,
        전업종지수요청,
        업종틱차트조회요청,
        업종분봉조회요청,
        업종일봉조회요청,
        업종주봉조회요청,
        업종월봉조회요청,
        업종현재가일별요청,
        입종년봉조회요청 = 20019,
        코스피200지수요청 = 50037,
        관심종목정보요청 = 100000,
        관심종목투자자정보요청,
        관심종목프로그램정보요청,
    }

    public enum KIWOOM_OPW_TR_CODE_DEF
    {
        예수금상세현황요청 = 1,
        일별추정예탁자산현황요청,
        추정자산조회요청,
        계좌평가현황요청,
        체결잔고요청,
        관리자별주문체결내역요청,
        계좌별주문체결내역상세요청,
        계좌별익일결제예정내역요청,
        계좌별주문체결현황요청,
        주문인출가능금액요청,
        증거금율별주문가능수량조회요청,
    }

    public enum KIWOOM_STATE_DEF
    {
        FAIL,
        OK,
    }

    public enum KIWOOM_nTypeFlag
    {
        주식종목,
        선물옵션종목 = 3
    }

    public enum KIWOOM_nOrderType
    {
        신규매수 = 1,
        신규매도,
        매수취소,
        매도취소,
        매수정정,
        매도정정
    }

    public enum KIWOOM_sCreditGb
    {
        신용매수_자기융자 = 3,
        신용매도_자기융자 = 33,
        신용매도_자기융자_합 = 99
    }

    public enum KIWOOM_sHogaGb
    {
        지정가 = 0,
        시장가 = 3,
        조건부지정가 = 5,
        최유리지정가 = 6,
        최우선지정가 = 7,
        지정가IOC = 10,
        시장가IOC = 13,
        최유리IOC = 16,
        지정가FOK = 20,
        시장가FOK = 23,
        최유리FOK = 26,
        장전시간외종가 = 61,
        시간외단일가매매 = 62,
        장후시간외종가 = 81,
    }

    public enum KIWOOM_lOrdKind
    {
        신규매매 = 1,
        정정,
        취소
    }

    public enum KIWOOM_sSlbyTp
    {
        매도 = 1,
        매수
    }

    public enum KIWOOM_sOrdTp
    {
        지정가 = 1,
        조건부지정가,
        시장가,
        최유리지정가,
        지정가IOC,
        지정가FOK,
        시장가IOC,
        시장가FOK,
        최유리지정가IOC,
        최유리지정가FOK
    }

    public enum ERROR_CODE_DEF
    {
        ORD_SYMCODE_EMPTY = -500,
        ORD_WRONG_ACCTINFO = -340,
        MIS_500CNT_EXC = -310,
        MIS_300CNT_EXC = -309,
        ORD_OVERFLOW_0 = -311,
        ORD_OVERFLOW_1 = -308,
        SEND_FAIL,
        MIS_3PER_EXC,
        MIS_1PER_EXC,
        MIS_5BILL_EXC,
        MIS_2BILL_EXC,
        OTHER_ACC_USE,
        ORD_WRONG_ACCTNO,
        ORD_WRONG_INPUT,
        REAL_CANCEL = -207,
        OVER_MAX_FID,
        DATA_RCV_FAIL,
        OVER_MAX_DATA,
        NO_DATA,
        RQ_STRING_FAIL,
        RQ_STRUCT_FAIL,
        SISE_OVERFLOW,
        SOCKET_CLOSED = -106,
        INPUT,
        MEMORY,
        FIREWALL,
        VERSION,
        CONNECT,
        LOGIN,
        COND_OVERFLOW = -13,
        COND_MISMATCH = -12,
        COND_NOTFOUND = -11,
        FAIL = -10,
        NONE = 0,
        FULL_TR_SEND,
    }

    public enum GetLoginInfo_Param_DEF
    {
        전체계좌_개수반환,
        전체계좌반환,//계좌별 구분은 ;
        사용자ID반환,
        사용자명반환,
        키보드보안_해지여부,
        방화벽설정여부,
        접속서버구분반환,
    }

    public enum 실시간FID
    {
        현재가_체결가_실시간종가 = 10,
        전일대비,
        등락율,
        누적거래량,
        누적거래대금_누적체결량,
        거래량_체결량,
        시가 = 16,
        고가,
        저가,
        체결시간 = 20,
        호가시간,
        예상체결가 = 23,
        예상체결수량 = 24,
        전일대비기호 = 25,
        전일거래량대비_계약_주,
        매도호가,
        매수호가,
        거래대금증감,
        전일거래량대비_비율,
        거래회전율,
        거래비용,

        매도호가1 = 41,
        매도호가2,
        매도호가3,
        매도호가4,
        매도호가5,
        매도호가6,
        매도호가7,
        매도호가8,
        매도호가9,
        매도호가10,

        매수호가1,
        매수호가2,
        매수호가3,
        매수호가4,
        매수호가5,
        매수호가6,
        매수호가7,
        매수호가8,
        매수호가9,
        매수호가10,

        매도호가수량1 = 61,
        매도호가수량2,
        매도호가수량3,
        매도호가수량4,
        매도호가수량5,
        매도호가수량6,
        매도호가수량7,
        매도호가수량8,
        매도호가수량9,
        매도호가수량10,

        매수호가수량1 = 71,
        매수호가수량2,
        매수호가수량3,
        매수호가수량4,
        매수호가수량5,
        매수호가수량6,
        매수호가수량7,
        매수호가수량8,
        매수호가수량9,
        매수호가수량10,

        매도호가직전대비1 = 81,
        매도호가직전대비2,
        매도호가직전대비3,
        매도호가직전대비4,
        매도호가직전대비5,
        매도호가직전대비6,
        매도호가직전대비7,
        매도호가직전대비8,
        매도호가직전대비9,
        매도호가직전대비10,

        매수호가직전대비1 = 91,
        매수호가직전대비2,
        매수호가직전대비3,
        매수호가직전대비4,
        매수호가직전대비5,
        매수호가직전대비6,
        매수호가직전대비7,
        매수호가직전대비8,
        매수호가직전대비9,
        매수호가직전대비10,

        매도호가_총잔량 = 121,
        매도호가_총잔량_직전대비,
        매수호가_총잔량 = 125,
        매수호가_총잔량_직전대비,

        순매수잔량 = 128,
        매수비율,
        시간외매도호가총잔량 = 131,
        시간외매도호가총잔량직전대비,
        시간외매수호가총잔량 = 135,
        시간외매수호가총잔량직전대비,
        순매도잔량 = 138,
        매도비율,

        예상체결가_전일종가_대비 = 200,
        예상체결가_전일종가_대비_등락율,
        장시작예상잔여시간 = 214,
        장운영구분 = 215,
        투자자별_TICKER,
        체결강도 = 228,
        예상체결가_전일정가_대비기호 = 238,
        장구분 = 290,
        예상체결가_1 = 291,
        예상체결량,
        예상체결가_전일대비기호,
        예상체결가_전일대비,
        예상체결가_전일대비등락율,
        전일거래량대비예상체결률 = 299,

        종목명 = 302,
        기준가 = 307,
        시가총액_억 = 311,

        상한가발생시간 = 567,
        하한가발생시간,

        LP매도호가수량1 = 621,
        LP매도호가수량2,
        LP매도호가수량3,
        LP매도호가수량4,
        LP매도호가수량5,
        LP매도호가수량6,
        LP매도호가수량7,
        LP매도호가수량8,
        LP매도호가수량9,
        LP매도호가수량10,
        LP매수호가수량1,
        LP매수호가수량2,
        LP매수호가수량3,
        LP매수호가수량4,
        LP매수호가수량5,
        LP매수호가수량6,
        LP매수호가수량7,
        LP매수호가수량8,
        LP매수호가수량9,
        LP매수호가수량10,
        K_O_접근도 = 691,

        전일_동시간_거래량_비율 = 851,

        대출일 = 916,
        신용구분 = 917,
        보유수량 = 930,
        매입단가,
        총매입가,
        주문가능수량,
        당일순매수량 = 945,
        매도_매수구분,
        당일총매도손익 = 950,
        신용금액 = 957,
        신용이자,
        만기일 = 918,
        당일실현손익_유가 = 990,
        당일실현손익률_유가,
        당일실현손익_신용,
        당일실현손익률_신용,
        담보대출수량 = 959,

        VI발동가격 = 1221,
        매매체결처리시각 = 1223,
        VI해제시각,
        VI적용구분,
        기준가격정적 = 1236,
        기준가격동적,
        괴리율정적,
        괴리율동적,
        VI발동가등락률 = 1489,
        VI발동횟수,

        실현손익 = 8019,
        종목코드_업종코드 = 9001,
        KOSPI_KOSDAQ_전체구분 = 9008,
        VI발동구분 = 9068,
        발동방향구분 = 9069,
        장전구분 = 9075,
        계좌번호 = 9201,
    }

    public class Define
    {
        public static Dictionary<ERROR_CODE_DEF, string> ERROR_CODE_DESC = new Dictionary<ERROR_CODE_DEF, string>
        {
            {ERROR_CODE_DEF.ORD_SYMCODE_EMPTY,"종목코드없음" },
            {ERROR_CODE_DEF.ORD_WRONG_ACCTINFO,"계좌정보없음"},
            {ERROR_CODE_DEF.MIS_500CNT_EXC,"주문수량 500계약 초과" },
            {ERROR_CODE_DEF.MIS_300CNT_EXC, "주문수량 300계약 초과"},
            {ERROR_CODE_DEF.ORD_OVERFLOW_0, "주문전송 과부하"},
            {ERROR_CODE_DEF.ORD_OVERFLOW_1, "주문전송 과부하"},
            {ERROR_CODE_DEF.SEND_FAIL, "주문전송 실패"},
            {ERROR_CODE_DEF.MIS_3PER_EXC, "주문수량이 총발행주수의 3%초과 오류"},
            {ERROR_CODE_DEF.MIS_1PER_EXC, "주문수량이 총발행주수의 1%초과 오류"},
            {ERROR_CODE_DEF.MIS_5BILL_EXC, "주문가격이 50억원을 초과"},
            {ERROR_CODE_DEF.MIS_2BILL_EXC, "주문가격이 20억원을 초과"},
            {ERROR_CODE_DEF.OTHER_ACC_USE, "타인계좌사용 오류"},
            {ERROR_CODE_DEF.ORD_WRONG_ACCTNO, "계좌 비밀번호 없음"},
            {ERROR_CODE_DEF.ORD_WRONG_INPUT, "입력값 오류"},
            {ERROR_CODE_DEF.REAL_CANCEL, "실시간 해제 오류"},
            {ERROR_CODE_DEF.OVER_MAX_FID, "조회 가능한 FID수 초과"},
            {ERROR_CODE_DEF.DATA_RCV_FAIL, "데이터수신 실패"},
            {ERROR_CODE_DEF.OVER_MAX_DATA, "조회 가능한 종목수 초과"},
            {ERROR_CODE_DEF.NO_DATA, "데이터 없음"},
            {ERROR_CODE_DEF.RQ_STRING_FAIL, "전문작성 입력값 오류"},
            {ERROR_CODE_DEF.RQ_STRUCT_FAIL, "전문작성 초기화 실패"},
            {ERROR_CODE_DEF.SISE_OVERFLOW, "시세조회 과부하"},
            {ERROR_CODE_DEF.SOCKET_CLOSED, "통신 연결 종료"},
            {ERROR_CODE_DEF.INPUT, "함수입력값 오류"},
            {ERROR_CODE_DEF.MEMORY, "메모리보호 실패"},
            {ERROR_CODE_DEF.FIREWALL, "개인방화벽 실패"},
            {ERROR_CODE_DEF.VERSION, "버전처리 실패"},
            {ERROR_CODE_DEF.CONNECT, "서버접속 실패"},
            {ERROR_CODE_DEF.LOGIN, "사용자정보 교환실패"},
            {ERROR_CODE_DEF.COND_OVERFLOW, "조건검색 조회요청 초과"},
            {ERROR_CODE_DEF.COND_MISMATCH, "조건번호와 조건식 틀림"},
            {ERROR_CODE_DEF.COND_NOTFOUND, "조건번호 없음"},
            {ERROR_CODE_DEF.FAIL, "실패"},
            {ERROR_CODE_DEF.NONE, "정상처리"},
        };

        public static Dictionary<GetLoginInfo_Param_DEF, string> GetLoginInfo_Param = new Dictionary<GetLoginInfo_Param_DEF, string>
        {
            {GetLoginInfo_Param_DEF.전체계좌_개수반환, "ACCOUNT_CNT"},
            {GetLoginInfo_Param_DEF.전체계좌반환, "ACCNO" },
            {GetLoginInfo_Param_DEF.사용자ID반환, "USER_ID" },
            {GetLoginInfo_Param_DEF.사용자명반환, "USER_NAME" },
            {GetLoginInfo_Param_DEF.키보드보안_해지여부, "KEY_BSECGB" },
            {GetLoginInfo_Param_DEF.방화벽설정여부,"FIREW_SECGB" },
            {GetLoginInfo_Param_DEF.접속서버구분반환,"GetServerGubun" }
        };

        public static 실시간FID[] FID주식시세 =
        {
            실시간FID.현재가_체결가_실시간종가,
            실시간FID.전일대비,
            실시간FID.등락율,
            실시간FID.매도호가,
            실시간FID.매수호가,
            실시간FID.누적거래량,
            실시간FID.누적거래대금_누적체결량,
            실시간FID.시가,
            실시간FID.고가,
            실시간FID.저가,
            실시간FID.전일대비기호,
            실시간FID.전일거래량대비_계약_주,
            실시간FID.거래대금증감,
            실시간FID.전일거래량대비_비율,
            실시간FID.거래회전율,
            실시간FID.거래비용,
            실시간FID.시가총액_억,
            실시간FID.상한가발생시간,
            실시간FID.하한가발생시간
        };

        public static 실시간FID[] FID주식체결 =
        {
            실시간FID.체결시간,
            실시간FID.현재가_체결가_실시간종가,
            실시간FID.전일대비,
            실시간FID.등락율,
            실시간FID.매도호가,
            실시간FID.매수호가,
            실시간FID.거래량_체결량,
            실시간FID.누적거래량,
            실시간FID.누적거래대금_누적체결량,
            실시간FID.시가,
            실시간FID.고가,
            실시간FID.저가,
            실시간FID.전일대비기호,
            실시간FID.전일거래량대비_계약_주,
            실시간FID.거래대금증감,
            실시간FID.전일거래량대비_비율,
            실시간FID.거래회전율,
            실시간FID.거래비용,
            실시간FID.체결강도,
            실시간FID.시가총액_억,
            실시간FID.장구분,
            실시간FID.상한가발생시간,
            실시간FID.하한가발생시간,
            실시간FID.전일_동시간_거래량_비율
        };

        public static 실시간FID[] FID주식우선호가 =
        {
            실시간FID.매도호가,
            실시간FID.매수호가
        };

        public static 실시간FID[] FID주식호가잔량 =
        {
            실시간FID.호가시간,
            실시간FID.매도호가1,
            실시간FID.매도호가수량1,
            실시간FID.매도호가직전대비1,
            실시간FID.매수호가1,
            실시간FID.매수호가수량1,
            실시간FID.매수호가직전대비1,
            실시간FID.매도호가2,
            실시간FID.매도호가수량2,
            실시간FID.매도호가직전대비2,
            실시간FID.매수호가2,
            실시간FID.매수호가수량2,
            실시간FID.매수호가직전대비2,
            실시간FID.매도호가3,
            실시간FID.매도호가수량3,
            실시간FID.매도호가직전대비3,
            실시간FID.매수호가3,
            실시간FID.매수호가수량3,
            실시간FID.매수호가직전대비3,
            실시간FID.매도호가4,
            실시간FID.매도호가수량4,
            실시간FID.매도호가직전대비4,
            실시간FID.매수호가4,
            실시간FID.매수호가수량4,
            실시간FID.매수호가직전대비4,
            실시간FID.매도호가5,
            실시간FID.매도호가수량5,
            실시간FID.매도호가직전대비5,
            실시간FID.매수호가5,
            실시간FID.매수호가수량5,
            실시간FID.매수호가직전대비5,
            실시간FID.매도호가6,
            실시간FID.매도호가수량6,
            실시간FID.매도호가직전대비6,
            실시간FID.매수호가6,
            실시간FID.매수호가수량6,
            실시간FID.매수호가직전대비6,
            실시간FID.매도호가7,
            실시간FID.매도호가수량7,
            실시간FID.매도호가직전대비7,
            실시간FID.매수호가7,
            실시간FID.매수호가수량7,
            실시간FID.매수호가직전대비7,
            실시간FID.매도호가8,
            실시간FID.매도호가수량8,
            실시간FID.매도호가직전대비8,
            실시간FID.매수호가8,
            실시간FID.매수호가수량8,
            실시간FID.매수호가직전대비8,
            실시간FID.매도호가9,
            실시간FID.매도호가수량9,
            실시간FID.매도호가직전대비9,
            실시간FID.매수호가9,
            실시간FID.매수호가수량9,
            실시간FID.매수호가직전대비9,
            실시간FID.매도호가10,
            실시간FID.매도호가수량10,
            실시간FID.매도호가직전대비10,
            실시간FID.매수호가10,
            실시간FID.매수호가수량10,
            실시간FID.매수호가직전대비10,
            실시간FID.매도호가_총잔량,
            실시간FID.매도호가_총잔량_직전대비,
            실시간FID.매수호가_총잔량,
            실시간FID.매수호가_총잔량_직전대비,
            실시간FID.예상체결가,
            실시간FID.예상체결수량,
            실시간FID.순매수잔량,
            실시간FID.매수비율,
            실시간FID.순매도잔량,
            실시간FID.매도비율,
            실시간FID.예상체결가_전일종가_대비,
            실시간FID.예상체결가_전일종가_대비_등락율,
            실시간FID.예상체결가_전일정가_대비기호,
            실시간FID.예상체결량,
            실시간FID.예상체결가_전일대비기호,
            실시간FID.예상체결가_전일대비,
            실시간FID.예상체결가_전일대비등락율,
            실시간FID.누적거래량,
            실시간FID.전일거래량대비예상체결률,
            실시간FID.장운영구분,
            실시간FID.투자자별_TICKER,
        };

        public static 실시간FID[] FID주식시간외호가 =
        {
            실시간FID.호가시간,
            실시간FID.시간외매도호가총잔량,
            실시간FID.시간외매도호가총잔량직전대비,
            실시간FID.시간외매수호가총잔량,
            실시간FID.시간외매수호가총잔량직전대비
        };

        public static 실시간FID[] FID장시작시간 =
        {
            실시간FID.장운영구분,
            실시간FID.체결시간,
            실시간FID.장시작예상잔여시간,
        };

        public static 실시간FID[] FDI주식당일거래원 =
        {

        };

        public static 실시간FID[] FIDVI발동해재 =
        {

        };

        public static 실시간FID[] FID잔고 =
        {
            실시간FID.계좌번호,
            실시간FID.종목코드_업종코드,
            실시간FID.신용구분,
            실시간FID.대출일,
            실시간FID.종목명,
            실시간FID.현재가_체결가_실시간종가,
            실시간FID.보유수량,
            실시간FID.매입단가,
            실시간FID.총매입가,
            실시간FID.주문가능수량,
            실시간FID.당일순매수량,
            실시간FID.매도_매수구분,
            실시간FID.당일총매도손익,
            실시간FID.매도호가,
            실시간FID.매수호가,
            실시간FID.기준가,
            실시간FID.실현손익,
            실시간FID.신용금액,
            실시간FID.신용이자,
            실시간FID.만기일,
            실시간FID.당일실현손익_유가,
            실시간FID.당일실현손익률_유가,
            실시간FID.당일실현손익_신용,
            실시간FID.당일실현손익률_신용,
            실시간FID.담보대출수량
        };


        public static string CreateTransactionCode(KIWOOM_OPT_TR_CODE_DEF trCode)
        {
            string header = "opt";

            if (trCode >= KIWOOM_OPT_TR_CODE_DEF.관심종목정보요청)
            {
                switch (trCode)
                {
                    case KIWOOM_OPT_TR_CODE_DEF.관심종목정보요청:
                        return "OPTKWFID";
                    case KIWOOM_OPT_TR_CODE_DEF.관심종목투자자정보요청:
                        return "OPTKWINV";
                    case KIWOOM_OPT_TR_CODE_DEF.관심종목프로그램정보요청:
                        return "OPTKWPRO";
                }
            }

            return header + ((int)trCode).ToString();
        }

        public static string CreateTransactionCode(KIWOOM_OPW_TR_CODE_DEF trCode)
        {
            string header = "opw";

            return header + ((int)trCode).ToString("D5");
        }

        public static string MakeFidList2String(실시간FID[] fids)
        {
            string result = string.Empty;
            for (int i = 0; i < fids.Length; i++)
            {
                result += ((int)fids[i]).ToString();
                if (i != fids.Length - 1)
                    result += ";";
            }
            return result;
        }
    }
}
