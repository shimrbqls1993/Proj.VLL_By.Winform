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

/**
 * 볼린저 밴드 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간 (기본값: 20)
 * @param {number} multiplier - 표준편차 승수 (기본값: 2)
 * @returns {Object} 중간선, 상단선, 하단선 데이터
 */
export const calculateBollingerBands = (data, period = 20, multiplier = 2) => {
  // SMA 계산 (중간선)
  const middle = calculateSMA(data, period);
  
  // 상단선과 하단선 계산
  const upper = [];
  const lower = [];
  
  for (let i = period - 1; i < data.length; i++) {
    // 표준편차 계산
    let sum = 0;
    for (let j = 0; j < period; j++) {
      const deviation = data[i - j].close - (middle[i - period + 1]?.value || 0);
      sum += deviation * deviation;
    }
    const stdDev = Math.sqrt(sum / period);
    
    // 상단선 = 중간선 + (표준편차 * 승수)
    upper.push({
      time: data[i].time,
      value: (middle[i - period + 1]?.value || 0) + (stdDev * multiplier)
    });
    
    // 하단선 = 중간선 - (표준편차 * 승수)
    lower.push({
      time: data[i].time,
      value: (middle[i - period + 1]?.value || 0) - (stdDev * multiplier)
    });
  }
  
  return { middle, upper, lower };
};

/**
 * 스토캐스틱 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} kPeriod - %K 기간 (기본값: 14)
 * @param {number} dPeriod - %D 기간 (기본값: 3)
 * @param {number} slowing - 슬로잉 기간 (기본값: 3)
 * @returns {Object} %K와 %D 라인 데이터
 */
export const calculateStochastic = (data, kPeriod = 14, dPeriod = 3, slowing = 3) => {
  const kLine = [];
  
  // %K 계산
  for (let i = kPeriod - 1; i < data.length; i++) {
    // 기간 내 최고가와 최저가 찾기
    let highestHigh = -Infinity;
    let lowestLow = Infinity;
    
    for (let j = 0; j < kPeriod; j++) {
      const high = data[i - j].high;
      const low = data[i - j].low;
      
      if (high > highestHigh) highestHigh = high;
      if (low < lowestLow) lowestLow = low;
    }
    
    // 슬로잉 적용
    let sumC = 0;
    let sumHL = 0;
    
    for (let j = 0; j < slowing; j++) {
      if (i - j < 0) continue;
      const close = data[i - j].close;
      sumC += close - lowestLow;
      sumHL += highestHigh - lowestLow;
    }
    
    // %K = (현재가 - 최저가) / (최고가 - 최저가) * 100
    const k = sumHL !== 0 ? (sumC / sumHL) * 100 : 0;
    
    kLine.push({
      time: data[i].time,
      value: k
    });
  }
  
  // %D = %K의 이동평균
  const dLine = [];
  for (let i = dPeriod - 1; i < kLine.length; i++) {
    let sum = 0;
    for (let j = 0; j < dPeriod; j++) {
      sum += kLine[i - j].value;
    }
    dLine.push({
      time: kLine[i].time,
      value: sum / dPeriod
    });
  }
  
  return { kLine, dLine };
};

/**
 * 파라볼릭 SAR 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} step - 가속 인자 (기본값: 0.02)
 * @param {number} max - 최대 가속 인자 (기본값: 0.2)
 * @returns {Array} SAR 데이터 배열
 */
export const calculateParabolicSAR = (data, step = 0.02, max = 0.2) => {
  if (data.length < 2) return [];
  
  const result = [];
  let isUptrend = data[1].close > data[0].close;
  let sar = isUptrend ? data[0].low : data[0].high;
  let extremePoint = isUptrend ? data[0].high : data[0].low;
  let accelerationFactor = step;
  
  // 첫 번째 데이터 포인트는 건너뜀
  for (let i = 1; i < data.length; i++) {
    // 이전 SAR 저장
    const prevSAR = sar;
    
    // SAR 계산
    sar = prevSAR + accelerationFactor * (extremePoint - prevSAR);
    
    // 추세 전환 확인
    let trendChanged = false;
    
    if (isUptrend) {
      // 상승 추세에서 SAR는 저가 아래에 위치
      if (sar > data[i].low) {
        isUptrend = false;
        trendChanged = true;
        sar = Math.max(...data.slice(Math.max(0, i - 2), i + 1).map(d => d.high));
        extremePoint = data[i].low;
        accelerationFactor = step;
      }
    } else {
      // 하락 추세에서 SAR는 고가 위에 위치
      if (sar < data[i].high) {
        isUptrend = true;
        trendChanged = true;
        sar = Math.min(...data.slice(Math.max(0, i - 2), i + 1).map(d => d.low));
        extremePoint = data[i].high;
        accelerationFactor = step;
      }
    }
    
    // 추세가 바뀌지 않았다면 EP와 AF 업데이트
    if (!trendChanged) {
      if (isUptrend) {
        if (data[i].high > extremePoint) {
          extremePoint = data[i].high;
          accelerationFactor = Math.min(accelerationFactor + step, max);
        }
      } else {
        if (data[i].low < extremePoint) {
          extremePoint = data[i].low;
          accelerationFactor = Math.min(accelerationFactor + step, max);
        }
      }
    }
    
    // 결과 저장
    result.push({
      time: data[i].time,
      value: sar
    });
  }
  
  return result;
};

/**
 * 엔벨로프 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간 (기본값: 20)
 * @param {number} upperPercentage - 상단 백분율 (기본값: 0.1)
 * @param {number} lowerPercentage - 하단 백분율 (기본값: 0.1)
 * @returns {Object} 중간선, 상단선, 하단선 데이터
 */
export const calculateEnvelope = (data, period = 20, upperPercentage = 0.1, lowerPercentage = 0.1) => {
  // SMA 계산 (중간선)
  const middle = calculateSMA(data, period);
  
  // 상단선과 하단선 계산
  const upper = middle.map(point => ({
    time: point.time,
    value: point.value * (1 + upperPercentage)
  }));
  
  const lower = middle.map(point => ({
    time: point.time,
    value: point.value * (1 - lowerPercentage)
  }));
  
  return { middle, upper, lower };
};

/**
 * 피봇 포인트 계산 (일일 피봇)
 * @param {Array} data - 캔들스틱 데이터 배열
 * @returns {Object} 피봇 포인트, 지지선, 저항선 데이터
 */
export const calculatePivotPoints = (data) => {
  if (data.length < 1) return {};
  
  // 가장 최근 데이터 사용
  const lastCandle = data[data.length - 1];
  const { high, low, close } = lastCandle;
  
  // 피봇 포인트 계산
  const pivot = (high + low + close) / 3;
  
  // 지지선 계산
  const s1 = (2 * pivot) - high;
  const s2 = pivot - (high - low);
  const s3 = low - 2 * (high - pivot);
  
  // 저항선 계산
  const r1 = (2 * pivot) - low;
  const r2 = pivot + (high - low);
  const r3 = high + 2 * (pivot - low);
  
  // 모든 데이터 포인트에 대해 동일한 값 적용
  const result = {
    pivot: data.map(d => ({ time: d.time, value: pivot })),
    r1: data.map(d => ({ time: d.time, value: r1 })),
    r2: data.map(d => ({ time: d.time, value: r2 })),
    r3: data.map(d => ({ time: d.time, value: r3 })),
    s1: data.map(d => ({ time: d.time, value: s1 })),
    s2: data.map(d => ({ time: d.time, value: s2 })),
    s3: data.map(d => ({ time: d.time, value: s3 }))
  };
  
  return result;
};

/**
 * 가격 채널 계산
 * @param {Array} data - 캔들스틱 데이터 배열
 * @param {number} period - 기간 (기본값: 20)
 * @returns {Object} 상단선, 중간선, 하단선 데이터
 */
export const calculatePriceChannel = (data, period = 20) => {
  const upper = [];
  const lower = [];
  const middle = [];
  
  for (let i = period - 1; i < data.length; i++) {
    // 기간 내 최고가와 최저가 찾기
    let highestHigh = -Infinity;
    let lowestLow = Infinity;
    
    for (let j = 0; j < period; j++) {
      const high = data[i - j].high;
      const low = data[i - j].low;
      
      if (high > highestHigh) highestHigh = high;
      if (low < lowestLow) lowestLow = low;
    }
    
    // 중간선 = (최고가 + 최저가) / 2
    const middleValue = (highestHigh + lowestLow) / 2;
    
    upper.push({
      time: data[i].time,
      value: highestHigh
    });
    
    lower.push({
      time: data[i].time,
      value: lowestLow
    });
    
    middle.push({
      time: data[i].time,
      value: middleValue
    });
  }
  
  return { upper, middle, lower };
}; 