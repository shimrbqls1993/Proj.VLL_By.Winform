@echo off
echo �ֽ� ���� ����Ʈ�� �������� ��...
cd scripts
if not exist "python_embedded" (
    echo Python Embedded not found. Running setup...
    call setup_embedded.bat
)
python_embedded\python.exe stock_list.py
cd ..

echo �� ���ø����̼��� �����մϴ�...
node start.js

pause 