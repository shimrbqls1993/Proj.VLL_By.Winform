# -*- coding: utf-8 -*-
import os
import json
from datetime import datetime
import pytz
import requests
from bs4 import BeautifulSoup
import pandas as pd

# 프로젝트 루트 디렉토리 경로 설정
ROOT_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

def get_today_date():
    """
    한국 시간 기준으로 오늘 날짜를 반환합니다.
    
    Returns:
    str: YYYYMMDD 형식의 날짜 문자열
    """
    kr_tz = pytz.timezone('Asia/Seoul')
    today = datetime.now(kr_tz)
    return today.strftime('%Y%m%d')

def check_existing_data(date):
    """
    지정된 날짜의 종목 리스트 데이터가 이미 존재하는지 확인합니다.
    
    Parameters:
    date (str): YYYYMMDD 형식의 날짜 문자열
    
    Returns:
    bool: 데이터가 존재하면 True, 없으면 False
    """
    data_dir = os.path.join(ROOT_DIR, "Data", "StockList")
    filename = f"stock_list_{date}.json"
    filepath = os.path.join(data_dir, filename)
    return os.path.exists(filepath)

def get_stock_list():
    """
    코스피와 코스닥의 상장 종목 리스트를 가져옵니다.
    
    Returns:
    dict: 코스피와 코스닥 종목 정보를 담은 딕셔너리
    """
    try:
        # 코스피 종목 리스트 가져오기
        kospi_url = "https://finance.naver.com/sise/sise_market_sum.naver?sosok=0"
        kosdaq_url = "https://finance.naver.com/sise/sise_market_sum.naver?sosok=1"
        
        def get_stocks_from_naver(url, market_type):
            response = requests.get(url)
            soup = BeautifulSoup(response.text, 'html.parser')
            stock_rows = soup.select('table.type_2 tr[onmouseover]')
            
            stocks = []
            for row in stock_rows:
                try:
                    cols = row.select('td')
                    if len(cols) > 1:
                        href = cols[1].select_one('a')['href']
                        code = href.split('=')[-1]
                        name = cols[1].text.strip()
                        
                        stock_info = {
                            "code": code,
                            "name": name,
                            "market": market_type,
                            "yahoo_code": f"{code}.{'KS' if market_type == 'KOSPI' else 'KQ'}"
                        }
                        stocks.append(stock_info)
                except Exception as e:
                    print(f"개별 종목 처리 중 오류: {e}")
                    continue
            
            return stocks
        
        kospi_info = get_stocks_from_naver(kospi_url, "KOSPI")
        kosdaq_info = get_stocks_from_naver(kosdaq_url, "KOSDAQ")
        
        return {
            "KOSPI": kospi_info,
            "KOSDAQ": kosdaq_info,
            "last_updated": get_today_date()
        }
    except Exception as e:
        print(f"종목 리스트 가져오기 실패: {e}")
        return {
            "KOSPI": [],
            "KOSDAQ": [],
            "last_updated": get_today_date()
        }

def save_stock_list(data):
    """
    종목 리스트를 JSON 파일로 저장합니다.
    
    Parameters:
    data (dict): 저장할 종목 리스트 데이터
    """
    data_dir = os.path.join(ROOT_DIR, "Data", "StockList")
    os.makedirs(data_dir, exist_ok=True)
    
    date = data["last_updated"]
    filename = f"stock_list_{date}.json"
    filepath = os.path.join(data_dir, filename)
    
    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    
    print(f"종목 리스트가 {filepath}에 저장되었습니다.")
    print(f"KOSPI: {len(data['KOSPI'])}개, KOSDAQ: {len(data['KOSDAQ'])}개 종목이 저장되었습니다.")

def main():
    """
    메인 함수: 오늘 날짜의 종목 리스트를 가져와서 저장합니다.
    이미 오늘 데이터가 있다면 실행하지 않습니다.
    """
    today = get_today_date()
    
    if check_existing_data(today):
        print(f"{today} 날짜의 종목 리스트가 이미 존재합니다.")
        return
    
    try:
        print("종목 리스트를 가져오는 중...")
        stock_list = get_stock_list()
        if len(stock_list["KOSPI"]) > 0 or len(stock_list["KOSDAQ"]) > 0:
            save_stock_list(stock_list)
            print("종목 리스트 저장이 완료되었습니다.")
        else:
            print("종목 리스트를 가져오는데 실패했습니다.")
    except Exception as e:
        print(f"오류 발생: {e}")

if __name__ == "__main__":
    main() 