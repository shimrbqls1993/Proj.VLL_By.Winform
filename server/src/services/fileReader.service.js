const fs = require('fs-extra');
const path = require('path');
require('dotenv').config();

class FileReaderService {
    constructor() {
        this.dataPath = path.join(__dirname, '../../../Data/CandleSticks');
        this.cache = new Map();
        this.cacheTimeout = 5000; // 5초 캐시
        console.log('Data path:', this.dataPath);
    }

    async readCandleStickData(code, timeFrame) {
        try {
            const filePath = path.join(this.dataPath, `${code}.json`);
            console.log('Reading file from:', filePath);
            
            // 캐시 확인
            const now = Date.now();
            const cached = this.cache.get(code);
            if (cached && (now - cached.timestamp) < this.cacheTimeout) {
                console.log('Using cached data for:', code);
                return cached.data[timeFrame] || [];
            }
            
            // 파일 읽기
            const data = await fs.readJson(filePath);
            
            // 캐시 업데이트
            this.cache.set(code, {
                data: data,
                timestamp: now
            });
            
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

    clearCache() {
        this.cache.clear();
        console.log('Cache cleared');
    }
}

module.exports = new FileReaderService();
