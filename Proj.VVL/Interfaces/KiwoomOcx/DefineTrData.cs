using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    /// <summary>
    ///  데이터 건수를 지정할 수 없고, 데이터 유무에따라 한번에 최대 900개가 조회됩니다.
    /// </summary>
    public enum 주식분봉차트조회요청{
        현재가,
        거래량,
        체결시간,
        시가,
        고가,
        저가,
        수정주가구분,
        수정비율,
        대업종구분,
        소업종구분,
        종목정보,
        수정주가이벤트,
        전일종가,
    }

    /// <summary>
    ///  수정주가이벤트 항목은 데이터 제공되지 않음. 데이터 건수를 지정할 수 없고, 데이터 유무에따라 한번에 최대 600개가 조회됩니다.
    /// </summary>
    public class 주식일봉차트조회요청_Input
    {
        public string 종목코드 { get; set; }
        public string 기준일자 { get; set; }
        public string 수정주가구분 { get; set; }
    }

    public enum 주식일봉차트조회요청_Output
    {
        종목코드,
        현재가,
        거래량,
        거래대금,
        일자,
        시가,
        고가,
        저가,
        수정주가구분,
        수정비율,
        대업종구분,
        소업종구분,
        종목정보,
        수정주가이벤트,
        전일종가,
        MAX
    }

    public enum 주식주봉차트조회요청
    {
        현재가,
        거래량,
        거래대금,
        일자,
        시가,
        고가,
        저가,
        수정주가구분,
        수정비율,
        대업종구분,
        소업종구분,
        종목정보,
        수정주가이벤트,
        전일종가
    }

    public class 주식차트조회요청
    {
        public int 현재가 { get; set; }
        public int 거래량 { get; set; }
        public int 거래대금 { get; set; }
        public string 체결시간 { get; set; }
        public int 시가 { get; set; }
        public int 고가 { get; set; }
        public int 저가 { get; set; }
    }

    public class DefineTrData
    {

    }
}
