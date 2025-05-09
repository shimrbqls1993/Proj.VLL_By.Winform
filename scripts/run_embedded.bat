@echo off
if not exist "python_embedded" (
    echo Python Embedded not found. Running setup...
    call setup_embedded.bat
)

echo Running stock data script...
python_embedded\python.exe stock_data.py
python_embedded\python.exe stock_list.py 