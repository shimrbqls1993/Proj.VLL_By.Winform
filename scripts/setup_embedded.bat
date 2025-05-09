@echo off
echo Downloading Python Embedded...
curl -o python.zip https://www.python.org/ftp/python/3.11.8/python-3.11.8-embed-amd64.zip

echo Extracting Python...
powershell Expand-Archive python.zip -DestinationPath python_embedded
del python.zip

echo Configuring Python...
powershell -Command "(Get-Content python_embedded/python311._pth) -replace '#import site', 'import site' | Set-Content python_embedded/python311._pth"

echo Downloading get-pip.py...
curl -o python_embedded/get-pip.py https://bootstrap.pypa.io/get-pip.py

echo Installing pip...
python_embedded\python.exe python_embedded\get-pip.py --no-warn-script-location

echo Installing required packages...
python_embedded\python.exe -m pip install --no-cache-dir --no-warn-script-location pandas requests yfinance pytz beautifulsoup4

echo Setup completed!
pause 