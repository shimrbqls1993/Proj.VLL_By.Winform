@echo off
cd /d %~dp0
echo Starting Stock Chart Application...

start /b cmd /c "cd server && ..\node\node.exe src/app.js"
cd stock-chart-web && ..\node\node.exe node_modules/react-scripts/scripts/start.js

pause 