/**
 * 지표 계산을 위한 공통 유틸리티 함수들
 */

/**
 * 단순 이동평균(SMA) 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간
 * @returns {Array} 계산된 SMA 데이터 배열
 */
export const calculateSMA = (data, period) => {
  const result = [];
  
  for (let i = 0; i < data.length; i++) {
    if (i < period - 1) continue;
    let sum = 0;
    for (let j = 0; j < period; j++) {
      sum += data[i - j].close;
    }
    result.push({
      time: data[i].time,
      value: sum / period
    });
  }
  
  return result;
};

/**
 * 가중 이동평균(WMA) 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간
 * @returns {Array} 계산된 WMA 데이터 배열
 */
export const calculateWMA = (data, period) => {
  const result = [];
  
  for (let i = period - 1; i < data.length; i++) {
    let sum = 0;
    let weightSum = 0;
    for (let j = 0; j < period; j++) {
      const weight = period - j;
      sum += data[i - j].close * weight;
      weightSum += weight;
    }
    result.push({
      time: data[i].time,
      value: sum / weightSum
    });
  }
  
  return result;
};

/**
 * 지수 이동평균(EMA) 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간
 * @returns {Array} 계산된 EMA 데이터 배열
 */
export const calculateEMA = (data, period) => {
  const result = [];
  const multiplier = 2 / (period + 1);
  let ema = data[0].close;
  
  for (let i = 1; i < data.length; i++) {
    ema = (data[i].close - ema) * multiplier + ema;
    if (i >= period - 1) {
      result.push({
        time: data[i].time,
        value: ema
      });
    }
  }
  
  return result;
};

/**
 * MACD 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} shortPeriod - 단기 EMA 기간 (기본값: 12)
 * @param {number} longPeriod - 장기 EMA 기간 (기본값: 26)
 * @param {number} signalPeriod - 시그널 라인 기간 (기본값: 9)
 * @returns {Object} MACD 라인, 시그널 라인, 히스토그램 데이터
 */
export const calculateMACD = (data, shortPeriod = 12, longPeriod = 26, signalPeriod = 9) => {
  // EMA 계산 함수
  const ema = (data, period) => {
    const k = 2 / (period + 1);
    const result = [{ time: data[0].time, value: data[0].close }];
    
    for (let i = 1; i < data.length; i++) {
      result.push({
        time: data[i].time,
        value: data[i].close * k + result[i-1].value * (1-k)
      });
    }
    return result;
  };

  const shortEMA = ema(data, shortPeriod);
  const longEMA = ema(data, longPeriod);
  
  // MACD 라인 계산 (단기 EMA - 장기 EMA)
  const macdLine = shortEMA.map((short, i) => ({
    time: short.time,
    value: short.value - longEMA[i].value
  }));

  return macdLine;
};

/**
 * RSI 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간 (기본값: 14)
 * @returns {Array} 계산된 RSI 데이터 배열
 */
export const calculateRSI = (data, period = 14) => {
  const changes = data.map((d, i) => {
    if (i === 0) return { gain: 0, loss: 0 };
    const change = d.close - data[i-1].close;
    return {
      gain: change > 0 ? change : 0,
      loss: change < 0 ? -change : 0
    };
  });

  const avgGain = changes.slice(1, period + 1).reduce((sum, c) => sum + c.gain, 0) / period;
  const avgLoss = changes.slice(1, period + 1).reduce((sum, c) => sum + c.loss, 0) / period;

  const result = [{
    time: data[period].time,
    value: 100 - (100 / (1 + avgGain / avgLoss))
  }];

  for (let i = period + 1; i < data.length; i++) {
    const change = changes[i];
    const newAvgGain = (avgGain * (period - 1) + change.gain) / period;
    const newAvgLoss = (avgLoss * (period - 1) + change.loss) / period;
    result.push({
      time: data[i].time,
      value: 100 - (100 / (1 + newAvgGain / newAvgLoss))
    });
  }

  return result;
}; 