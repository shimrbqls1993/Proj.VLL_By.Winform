/**
 * ��ǥ ����� ���� ���� ��ƿ��Ƽ �Լ���
 */

/**
 * �ܼ� �̵����(SMA) ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ
 * @returns {Array} ���� SMA ������ �迭
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
 * ���� �̵����(WMA) ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ
 * @returns {Array} ���� WMA ������ �迭
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
 * ���� �̵����(EMA) ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ
 * @returns {Array} ���� EMA ������ �迭
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
 * MACD ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} shortPeriod - �ܱ� EMA �Ⱓ (�⺻��: 12)
 * @param {number} longPeriod - ��� EMA �Ⱓ (�⺻��: 26)
 * @param {number} signalPeriod - �ñ׳� ���� �Ⱓ (�⺻��: 9)
 * @returns {Object} MACD ����, �ñ׳� ����, ������׷� ������
 */
export const calculateMACD = (data, shortPeriod = 12, longPeriod = 26, signalPeriod = 9) => {
  // EMA ��� �Լ�
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
  
  // MACD ���� ��� (�ܱ� EMA - ��� EMA)
  const macdLine = shortEMA.map((short, i) => ({
    time: short.time,
    value: short.value - longEMA[i].value
  }));

  return macdLine;
};

/**
 * RSI ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ (�⺻��: 14)
 * @returns {Array} ���� RSI ������ �迭
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
 * ������ ��� ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ (�⺻��: 20)
 * @param {number} multiplier - ǥ������ �¼� (�⺻��: 2)
 * @returns {Object} �߰���, ��ܼ�, �ϴܼ� ������
 */
export const calculateBollingerBands = (data, period = 20, multiplier = 2) => {
  // SMA ��� (�߰���)
  const middle = calculateSMA(data, period);
  
  // ��ܼ��� �ϴܼ� ���
  const upper = [];
  const lower = [];
  
  for (let i = period - 1; i < data.length; i++) {
    // ǥ������ ���
    let sum = 0;
    for (let j = 0; j < period; j++) {
      const deviation = data[i - j].close - (middle[i - period + 1]?.value || 0);
      sum += deviation * deviation;
    }
    const stdDev = Math.sqrt(sum / period);
    
    // ��ܼ� = �߰��� + (ǥ������ * �¼�)
    upper.push({
      time: data[i].time,
      value: (middle[i - period + 1]?.value || 0) + (stdDev * multiplier)
    });
    
    // �ϴܼ� = �߰��� - (ǥ������ * �¼�)
    lower.push({
      time: data[i].time,
      value: (middle[i - period + 1]?.value || 0) - (stdDev * multiplier)
    });
  }
  
  return { middle, upper, lower };
};

/**
 * ����ĳ��ƽ ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} kPeriod - %K �Ⱓ (�⺻��: 14)
 * @param {number} dPeriod - %D �Ⱓ (�⺻��: 3)
 * @param {number} slowing - ������ �Ⱓ (�⺻��: 3)
 * @returns {Object} %K�� %D ���� ������
 */
export const calculateStochastic = (data, kPeriod = 14, dPeriod = 3, slowing = 3) => {
  const kLine = [];
  
  // %K ���
  for (let i = kPeriod - 1; i < data.length; i++) {
    // �Ⱓ �� �ְ��� ������ ã��
    let highestHigh = -Infinity;
    let lowestLow = Infinity;
    
    for (let j = 0; j < kPeriod; j++) {
      const high = data[i - j].high;
      const low = data[i - j].low;
      
      if (high > highestHigh) highestHigh = high;
      if (low < lowestLow) lowestLow = low;
    }
    
    // ������ ����
    let sumC = 0;
    let sumHL = 0;
    
    for (let j = 0; j < slowing; j++) {
      if (i - j < 0) continue;
      const close = data[i - j].close;
      sumC += close - lowestLow;
      sumHL += highestHigh - lowestLow;
    }
    
    // %K = (���簡 - ������) / (�ְ� - ������) * 100
    const k = sumHL !== 0 ? (sumC / sumHL) * 100 : 0;
    
    kLine.push({
      time: data[i].time,
      value: k
    });
  }
  
  // %D = %K�� �̵����
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
 * �Ķ󺼸� SAR ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} step - ���� ���� (�⺻��: 0.02)
 * @param {number} max - �ִ� ���� ���� (�⺻��: 0.2)
 * @returns {Array} SAR ������ �迭
 */
export const calculateParabolicSAR = (data, step = 0.02, max = 0.2) => {
  if (data.length < 2) return [];
  
  const result = [];
  let isUptrend = data[1].close > data[0].close;
  let sar = isUptrend ? data[0].low : data[0].high;
  let extremePoint = isUptrend ? data[0].high : data[0].low;
  let accelerationFactor = step;
  
  // ù ��° ������ ����Ʈ�� �ǳʶ�
  for (let i = 1; i < data.length; i++) {
    // ���� SAR ����
    const prevSAR = sar;
    
    // SAR ���
    sar = prevSAR + accelerationFactor * (extremePoint - prevSAR);
    
    // �߼� ��ȯ Ȯ��
    let trendChanged = false;
    
    if (isUptrend) {
      // ��� �߼����� SAR�� ���� �Ʒ��� ��ġ
      if (sar > data[i].low) {
        isUptrend = false;
        trendChanged = true;
        sar = Math.max(...data.slice(Math.max(0, i - 2), i + 1).map(d => d.high));
        extremePoint = data[i].low;
        accelerationFactor = step;
      }
    } else {
      // �϶� �߼����� SAR�� �� ���� ��ġ
      if (sar < data[i].high) {
        isUptrend = true;
        trendChanged = true;
        sar = Math.min(...data.slice(Math.max(0, i - 2), i + 1).map(d => d.low));
        extremePoint = data[i].high;
        accelerationFactor = step;
      }
    }
    
    // �߼��� �ٲ��� �ʾҴٸ� EP�� AF ������Ʈ
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
    
    // ��� ����
    result.push({
      time: data[i].time,
      value: sar
    });
  }
  
  return result;
};

/**
 * �������� ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ (�⺻��: 20)
 * @param {number} upperPercentage - ��� ����� (�⺻��: 0.1)
 * @param {number} lowerPercentage - �ϴ� ����� (�⺻��: 0.1)
 * @returns {Object} �߰���, ��ܼ�, �ϴܼ� ������
 */
export const calculateEnvelope = (data, period = 20, upperPercentage = 0.1, lowerPercentage = 0.1) => {
  // SMA ��� (�߰���)
  const middle = calculateSMA(data, period);
  
  // ��ܼ��� �ϴܼ� ���
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
 * �Ǻ� ����Ʈ ��� (���� �Ǻ�)
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @returns {Object} �Ǻ� ����Ʈ, ������, ���׼� ������
 */
export const calculatePivotPoints = (data) => {
  if (data.length < 1) return {};
  
  // ���� �ֱ� ������ ���
  const lastCandle = data[data.length - 1];
  const { high, low, close } = lastCandle;
  
  // �Ǻ� ����Ʈ ���
  const pivot = (high + low + close) / 3;
  
  // ������ ���
  const s1 = (2 * pivot) - high;
  const s2 = pivot - (high - low);
  const s3 = low - 2 * (high - pivot);
  
  // ���׼� ���
  const r1 = (2 * pivot) - low;
  const r2 = pivot + (high - low);
  const r3 = high + 2 * (pivot - low);
  
  // ��� ������ ����Ʈ�� ���� ������ �� ����
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
 * ���� ä�� ���
 * @param {Array} data - ĵ�齺ƽ ������ �迭
 * @param {number} period - �Ⱓ (�⺻��: 20)
 * @returns {Object} ��ܼ�, �߰���, �ϴܼ� ������
 */
export const calculatePriceChannel = (data, period = 20) => {
  const upper = [];
  const lower = [];
  const middle = [];
  
  for (let i = period - 1; i < data.length; i++) {
    // �Ⱓ �� �ְ��� ������ ã��
    let highestHigh = -Infinity;
    let lowestLow = Infinity;
    
    for (let j = 0; j < period; j++) {
      const high = data[i - j].high;
      const low = data[i - j].low;
      
      if (high > highestHigh) highestHigh = high;
      if (low < lowestLow) lowestLow = low;
    }
    
    // �߰��� = (�ְ� + ������) / 2
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