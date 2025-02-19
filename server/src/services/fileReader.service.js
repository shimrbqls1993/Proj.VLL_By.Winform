const fs = require('fs-extra');
const path = require('path');

class FileReaderService {
    constructor() {
        // 현재 C# 프로그램의 Data/CandleSticks 경로 설정
        this.dataPath = 'D:/Proj.VLL_By.Winform-main/Data/CandleSticks';
    }

    async readCandleStickData(code, timeFrame) {
        try {
            const filePath = path.join(this.dataPath, `${code}.json`);
            console.log('Reading file from:', filePath);
            
            const data = await fs.readJson(filePath);
            
            // timeFrame에 따라 데이터 선택
            switch(timeFrame) {
                case 'WEEK': return data.WEEK || [];
                case 'DAY': return data.DAY || [];
                case 'HOUR': return data.HOUR || [];
                case 'MIN_5': return data.MIN_5 || [];
                case 'MIN_1': return data.MIN_1 || [];
                default: return data.DAY || [];
            }
        } catch (error) {
            console.error('Error reading file:', error);
            throw new Error(`Failed to read candlestick data: ${error.message}`);
        }
    }
}

module.exports = new FileReaderService();
