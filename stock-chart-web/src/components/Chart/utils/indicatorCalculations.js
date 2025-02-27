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