# -*- coding: utf-8 -*-
import yfinance as yf
import pandas as pd
import json
import os
from datetime import datetime, timedelta
import argparse

# 프로젝트 루트 디렉토리 설정
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

def get_stock_data(ticker, days=600, interval='1d'):
    """
    야후 파이넌스에서 주식 데이터를 가져옵니다.
    
    Parameters:
    ticker (str): 주식 티커 심볼 (예: 'AAPL', '005930.KS' 등)
    days (int): 가져올 데이터의 일수
    interval (str): 데이터 간격 (예: '1d', '1wk', '1mo')
    
    Returns:
    pandas.DataFrame: 주식 데이터 (Date, Open, High, Low, Close, Volume)
    """
    print(f"Fetching data for ticker: {ticker}")
    
    # 오늘 날짜 계산
    today = datetime.now()
    # 주말인 경우 금요일로 조정
    while today.weekday() > 4:  # 5는 토요일, 6은 일요일
        today = today - timedelta(days=1)
    
    # 시작 날짜 계산 (오늘 + 1일 추가하여 어제부터 시작)
    start_date = today - timedelta(days=days + 1)
    end_date = today - timedelta(days=1)  # 어제까지
    
    print(f"Fetching data from {start_date.strftime('%Y-%m-%d')} to {end_date.strftime('%Y-%m-%d')}")
    
    # 주식 데이터 가져오기
    stock = yf.Ticker(ticker)
    df = stock.history(start=start_date, end=end_date, interval=interval)
    print(f"Retrieved {len(df)} rows of data")
    
    # 인덱스를 Date 열로 변환
    df = df.reset_index()
    
    # 필요한 열만 선택 (Date, Open, High, Low, Close, Volume)
    df = df[['Date', 'Open', 'High', 'Low', 'Close', 'Volume']]
    
    return df

def save_stock_data(df, ticker, file_format='json'):
    """
    주식 데이터를 파일로 저장합니다.
    
    Parameters:
    df (pandas.DataFrame): 저장할 주식 데이터
    ticker (str): 주식 티커 심볼
    file_format (str): 저장 형식 ('csv', 'excel', 또는 'json')
    """
    # 티커 심볼에서 .KS 등의 접미사 제거
    clean_ticker = ticker.split('.')[0]
    print(f"Clean ticker: {clean_ticker}")
    
    if file_format.lower() == 'csv':
        today = datetime.now().strftime('%Y%m%d')
        filename = f"{ticker}_{today}.csv"
        df.to_csv(filename, index=False)
        print(f"데이터가 {filename} 파일로 저장되었습니다.")
    elif file_format.lower() == 'excel':
        today = datetime.now().strftime('%Y%m%d')
        filename = f"{ticker}_{today}.xlsx"
        df.to_excel(filename, index=False)
        print(f"데이터가 {filename} 파일로 저장되었습니다.")
    elif file_format.lower() == 'json':
        # 웹 차트용 JSON 형식으로 변환
        json_data = convert_to_chart_json(df, ticker)
        
        # Data/CandleSticks 폴더 경로 설정 (절대 경로 사용)
        data_dir = os.path.join(PROJECT_ROOT, "Data", "CandleSticks")
        os.makedirs(data_dir, exist_ok=True)
        print(f"Data directory: {data_dir}")
        
        # 파일 저장
        json_path = os.path.join(data_dir, f"{clean_ticker}.json")
        print(f"Saving to: {json_path}")
        
        # 기존 파일이 있는지 확인
        if os.path.exists(json_path):
            try:
                with open(json_path, 'r', encoding='utf-8') as f:
                    existing_data = json.load(f)
                print("Found existing file, updating data")
            except Exception as e:
                print(f"Error reading existing file: {e}")
                existing_data = {"WEEK": [], "DAY": [], "HOUR": [], "MIN_5": [], "MIN_1": []}
        else:
            print("Creating new file")
            existing_data = {"WEEK": [], "DAY": [], "HOUR": [], "MIN_5": [], "MIN_1": []}
        
        # 새 데이터 추가 (일봉 데이터는 DAY에 저장)
        existing_data["DAY"] = json_data["DAY"]
        
        try:
            # 파일 저장
            with open(json_path, 'w', encoding='utf-8') as f:
                json.dump(existing_data, f, ensure_ascii=False, indent=2)
            print(f"Successfully saved data to {json_path}")
        except Exception as e:
            print(f"Error saving file: {e}")
            raise
    else:
        print("지원하지 않는 파일 형식입니다. 'csv', 'excel', 또는 'json'을 사용하세요.")

def convert_to_chart_json(df, ticker):
    """
    DataFrame을 차트 라이브러리에서 사용할 수 있는 JSON 형식으로 변환합니다.
    
    Parameters:
    df (pandas.DataFrame): 변환할 주식 데이터
    ticker (str): 주식 티커 심볼
    
    Returns:
    dict: 차트 라이브러리에서 사용할 수 있는 JSON 형식의 데이터
    """
    # 날짜 형식 변환
    df['Datetime'] = df['Date'].dt.strftime('%Y%m%d')
    
    # JSON 형식으로 변환
    candles = []
    for _, row in df.iterrows():
        candle = {
            "Datetime": row['Datetime'],
            "Close": float(row['Close']),
            "Open": float(row['Open']),
            "High": float(row['High']),
            "Low": float(row['Low']),
            "Volume": float(row['Volume'])
        }
        candles.append(candle)
    
    # 결과 JSON 구조
    result = {
        "WEEK": [],
        "DAY": candles,
        "HOUR": [],
        "MIN_5": [],
        "MIN_1": []
    }
    
    return result

def get_stock_info(ticker):
    """
    야후 파이넌스에서 종목의 기본 정보를 가져옵니다.
    
    Parameters:
    ticker (str): 주식 티커 심볼 (예: '005930.KS')
    
    Returns:
    dict: 종목 기본 정보
    """
    print(f"Fetching stock info for: {ticker}")
    stock = yf.Ticker(ticker)
    
    # 기본 정보 가져오기
    info = stock.info
    
    # 필요한 정보 추출
    stock_info = {
        "종목코드": ticker.split('.')[0],
        "종목명": info.get('longName', ''),
        "현재가": info.get('regularMarketPrice', 0),
        "등락률": round((info.get('regularMarketChangePercent', 0) * 100), 2),
        "거래량": info.get('regularMarketVolume', 0),
        "시가총액": info.get('marketCap', 0),
        "외국인지분율": info.get('floatShares', 0) / info.get('sharesOutstanding', 1) * 100 if info.get('floatShares') else 0,
        "52주_최고": info.get('fiftyTwoWeekHigh', 0),
        "52주_최저": info.get('fiftyTwoWeekLow', 0),
        "업데이트_시각": datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    }
    
    return stock_info

def save_stock_info(stock_info):
    """
    종목 기본 정보를 JSON 파일로 저장합니다.
    """
    # Data/StockData 폴더 확인 및 생성
    data_dir = os.path.join(PROJECT_ROOT, "Data", "StockData")
    os.makedirs(data_dir, exist_ok=True)
    
    # 파일 저장
    json_path = os.path.join(data_dir, f"{stock_info['종목코드']}.json")
    print(f"Saving stock info to: {json_path}")
    
    try:
        with open(json_path, 'w', encoding='utf-8') as f:
            json.dump(stock_info, f, ensure_ascii=False, indent=2)
        print(f"Successfully saved stock info to {json_path}")
    except Exception as e:
        print(f"Error saving stock info: {e}")
        raise

def main():
    """
    메인 함수: 커맨드라인 인자를 처리하고 주식 데이터를 가져와 저장합니다.
    """
    parser = argparse.ArgumentParser(description='주식 데이터를 가져와서 저장합니다.')
    parser.add_argument('--code', type=str, help='종목 코드')
    parser.add_argument('--days', type=int, default=600, help='가져올 데이터의 일수')
    parser.add_argument('--format', type=str, default='json', choices=['csv', 'excel', 'json'], help='저장 형식')
    
    args = parser.parse_args()
    
    if args.code:
        # 종목 코드가 주어진 경우
        if not args.code.endswith(('.KS', '.KQ')):
            # 코스피/코스닥 구분을 위해 종목 리스트 확인
            today = datetime.now().strftime('%Y%m%d')
            stock_list_path = os.path.join(PROJECT_ROOT, "Data", "StockList", f"stock_list_{today}.json")
            print(f"Looking for stock list at: {stock_list_path}")
            
            try:
                with open(stock_list_path, 'r', encoding='utf-8') as f:
                    stock_list = json.load(f)
                
                print(f"Checking market for code: {args.code}")
                # 종목이 어느 시장에 속하는지 확인
                kospi_codes = [stock['code'] for stock in stock_list['KOSPI']]
                if args.code in kospi_codes:
                    ticker = f"{args.code}.KS"
                    print(f"Found in KOSPI: {ticker}")
                else:
                    ticker = f"{args.code}.KQ"
                    print(f"Assumed KOSDAQ: {ticker}")
            except Exception as e:
                print(f"Error reading stock list: {e}")
                # 종목 리스트 파일이 없는 경우 기본적으로 코스피로 가정
                ticker = f"{args.code}.KS"
                print(f"Using default market (KOSPI): {ticker}")
        else:
            ticker = args.code
            print(f"Using provided ticker: {ticker}")
    else:
        # 대화형 모드
        ticker = input("주식 티커 심볼을 입력하세요 (예: 'AAPL', '005930.KS'): ")
    
    try:
        # 종목 기본 정보 가져오기 및 저장
        print(f"{ticker}의 기본 정보를 가져오는 중...")
        stock_info = get_stock_info(ticker)
        save_stock_info(stock_info)
        
        # 차트 데이터 가져오기 및 저장
        print(f"{ticker}의 차트 데이터를 가져오는 중...")
        df = get_stock_data(ticker, days=args.days)
        print(f"데이터 행 수: {len(df)}")
        save_stock_data(df, ticker, args.format)
            
    except Exception as e:
        print(f"오류 발생: {e}")
        print(f"스택 트레이스:")
        import traceback
        traceback.print_exc()
        exit(1)

if __name__ == "__main__":
    main() 