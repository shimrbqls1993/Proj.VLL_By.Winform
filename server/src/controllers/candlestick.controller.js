const fileReaderService = require('../services/fileReader.service');
const moment = require('moment');

class CandlestickController {
    async getCandleStickData(req, res) {
        try {
            const { code, timeFrame } = req.params;
            const data = await fileReaderService.readCandleStickData(code, timeFrame);
            
            // 데이터 포맷 변환
            const formattedData = data.map(candle => ({
                time: moment(candle.Datetime).unix(),
                open: candle.Open,
                high: candle.High,
                low: candle.Low,
                close: candle.Close,
                volume: candle.Volume
            }));

            res.json(formattedData);
        } catch (error) {
            res.status(500).json({ error: error.message });
        }
    }
}

module.exports = new CandlestickController();
