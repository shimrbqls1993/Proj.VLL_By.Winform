@echo off
echo 주식 종목 리스트를 가져오는 중...
cd scripts
if not exist "python_embedded" (
    echo Python Embedded not found. Running setup...
    call setup_embedded.bat
)
python_embedded\python.exe stock_list.py
cd ..

echo 웹 애플리케이션을 시작합니다...
node start.js

pause 