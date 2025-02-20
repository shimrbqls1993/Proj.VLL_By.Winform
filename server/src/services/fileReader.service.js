const fs = require('fs-extra');
const path = require('path');
require('dotenv').config();

class FileReaderService {
    constructor() {
        this.dataPath = path.join(__dirname, '../../../Data/CandleSticks');
        console.log('Data path:', this.dataPath);
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
