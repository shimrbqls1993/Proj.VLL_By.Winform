const fs = require('fs').promises;
const path = require('path');
const { spawn } = require('child_process');

class StockController {
    async searchStocks(req, res) {
        try {
            const { query } = req.query;
            if (!query) {
                return res.json({ stocks: [] });
            }

            // 가장 최신 종목 리스트 파일 찾기
            const stockListDir = path.join(__dirname, '../../../Data/StockList');
            const files = await fs.readdir(stockListDir);
            const stockListFiles = files.filter(file => file.startsWith('stock_list_'));
            const latestFile = stockListFiles.sort().reverse()[0];

            // 종목 리스트 읽기
            const stockListPath = path.join(stockListDir, latestFile);
            const stockList = JSON.parse(await fs.readFile(stockListPath, 'utf-8'));

            // 검색어로 필터링 (종목 코드 또는 종목명에 검색어가 포함된 경우)
            const searchResult = [
                ...stockList.KOSPI.filter(stock => 
                    stock.code.includes(query) || 
                    stock.name.toLowerCase().includes(query.toLowerCase())
                ),
                ...stockList.KOSDAQ.filter(stock => 
                    stock.code.includes(query) || 
                    stock.name.toLowerCase().includes(query.toLowerCase())
                )
            ];

            res.json({ stocks: searchResult });
        } catch (error) {
            console.error('Search error:', error);
            res.status(500).json({ error: 'Failed to search stocks' });
        }
    }

    async fetchStockData(req, res) {
        try {
            const { code } = req.params;
            
            // Python 스크립트 경로를 절대 경로로 설정
            const scriptPath = path.join(__dirname, '../../../scripts/stock_data.py');
            console.log('Executing Python script:', scriptPath);
            console.log('Stock code:', code);

            // Python 스크립트 실행
            const pythonProcess = spawn('python', [
                scriptPath,
                '--code', code,
                '--days', '600',
                '--format', 'json'
            ]);

            let result = '';
            let error = '';

            pythonProcess.stdout.on('data', (data) => {
                result += data.toString();
                console.log('Python script output:', data.toString());
            });

            pythonProcess.stderr.on('data', (data) => {
                error += data.toString();
                console.error('Python script error:', data.toString());
            });

            pythonProcess.on('close', (code) => {
                if (code !== 0) {
                    console.error('Python script failed with code:', code);
                    console.error('Error output:', error);
                    return res.status(500).json({ error: `Failed to fetch stock data: ${error}` });
                }
                console.log('Python script completed successfully');
                res.json({ message: 'Stock data fetched successfully', result });
            });
        } catch (error) {
            console.error('Fetch error:', error);
            res.status(500).json({ error: `Failed to fetch stock data: ${error.message}` });
        }
    }

    async getStockInfo(req, res) {
        try {
            const { code } = req.params;
            
            // 종목 정보 파일 경로
            const infoPath = path.join(__dirname, '../../../Data/StockData', `${code}.json`);
            console.log('Looking for stock info at:', infoPath);
            
            try {
                // 파일에서 종목 정보 읽기
                const data = await fs.readFile(infoPath, 'utf-8');
                const stockInfo = JSON.parse(data);
                
                // 마지막 업데이트 시각 확인
                const lastUpdate = new Date(stockInfo.업데이트_시각);
                const now = new Date();
                const diffMinutes = (now - lastUpdate) / 1000 / 60;
                
                // 5분이 지났다면 새로운 데이터 가져오기
                if (diffMinutes > 5) {
                    console.log('Stock info is outdated, fetching new data...');
                    // Python 스크립트로 새로운 데이터 가져오기
                    const pythonProcess = spawn('python', [
                        path.join(__dirname, '../../../scripts/stock_data.py'),
                        '--code', code,
                        '--days', '600',
                        '--format', 'json'
                    ]);

                    let pythonError = '';
                    
                    pythonProcess.stderr.on('data', (data) => {
                        pythonError += data.toString();
                        console.error('Python script error:', data.toString());
                    });

                    pythonProcess.on('close', async (code) => {
                        try {
                            if (code === 0) {
                                // 새로운 데이터를 다시 읽어서 응답
                                const newData = await fs.readFile(infoPath, 'utf-8');
                                const newStockInfo = JSON.parse(newData);
                                console.log('Successfully updated stock info');
                                res.json(newStockInfo);
                            } else {
                                // 에러가 발생했더라도 기존 데이터 반환
                                console.log('Failed to update stock info, using cached data');
                                res.json(stockInfo);
                            }
                        } catch (err) {
                            console.error('Error reading updated stock info:', err);
                            res.json(stockInfo);
                        }
                    });
                } else {
                    // 캐시된 데이터 반환
                    console.log('Using cached stock info');
                    res.json(stockInfo);
                }
            } catch (error) {
                console.error('Error reading stock info:', error);
                // 파일이 없거나 읽기 실패 시 새로운 데이터 가져오기
                const pythonProcess = spawn('python', [
                    path.join(__dirname, '../../../scripts/stock_data.py'),
                    '--code', code,
                    '--days', '600',
                    '--format', 'json'
                ]);

                let pythonError = '';
                
                pythonProcess.stderr.on('data', (data) => {
                    pythonError += data.toString();
                    console.error('Python script error:', data.toString());
                });
                
                pythonProcess.on('close', async (code) => {
                    if (code === 0) {
                        try {
                            const data = await fs.readFile(infoPath, 'utf-8');
                            const stockInfo = JSON.parse(data);
                            console.log('Successfully fetched new stock info');
                            res.json(stockInfo);
                        } catch (err) {
                            console.error('Error reading new stock info:', err);
                            res.status(500).json({ 
                                error: 'Failed to read new stock data',
                                details: err.message
                            });
                        }
                    } else {
                        console.error('Failed to fetch stock data:', pythonError);
                        res.status(500).json({ 
                            error: 'Failed to fetch stock data',
                            details: pythonError
                        });
                    }
                });
            }
        } catch (error) {
            console.error('Stock info error:', error);
            res.status(500).json({ 
                error: 'Failed to get stock info',
                details: error.message
            });
        }
    }
}

module.exports = new StockController(); 