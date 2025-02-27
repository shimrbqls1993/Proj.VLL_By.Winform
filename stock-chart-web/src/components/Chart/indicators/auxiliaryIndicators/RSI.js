import React from 'react';
import { calculateRSI } from '../../utils/indicatorCalculations';

/**
 * RSI(Relative Strength Index) 지표
 */
const RSI = {
  // 기본 설정
  defaultSettings: {
    period: 14,
    overbought: 70,
    oversold: 30
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const handleSettingChange = (field, value) => {
      onSettingsChange({
        ...settings,
        [field]: parseInt(value)
      });
    };

    return (
      <div className="rsi-settings">
        <div className="setting-group">
          <label>
            기간:
            <input
              type="number"
              value={settings.period}
              onChange={(e) => handleSettingChange('period', e.target.value)}
              min="1"
            />
          </label>
        </div>
        <div className="setting-group">
          <label>
            과매수 수준:
            <input
              type="number"
              value={settings.overbought}
              onChange={(e) => handleSettingChange('overbought', e.target.value)}
              min="50"
              max="100"
            />
          </label>
        </div>
        <div className="setting-group">
          <label>
            과매도 수준:
            <input
              type="number"
              value={settings.oversold}
              onChange={(e) => handleSettingChange('oversold', e.target.value)}
              min="0"
              max="50"
            />
          </label>
        </div>
      </div>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data, settings) => {
    return calculateRSI(data, settings.period);
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings = RSI.defaultSettings) => {
    const rsiData = RSI.calculate(data, settings);
    
    const rsiSeries = chart.addLineSeries({
      color: '#9C27B0',
      lineWidth: 1,
      title: 'RSI',
      priceScaleId: 'rsi',
      scaleMargins: {
        top: 0.7,
        bottom: 0.2,
      },
    });
    
    rsiSeries.setData(rsiData);
    
    // 과매수/과매도 수준 라인 추가 (선택적)
    const overboughtLine = chart.addLineSeries({
      color: 'rgba(255, 0, 0, 0.5)',
      lineWidth: 1,
      lineStyle: 2, // 점선
      priceScaleId: 'rsi',
    });
    
    const oversoldLine = chart.addLineSeries({
      color: 'rgba(0, 255, 0, 0.5)',
      lineWidth: 1,
      lineStyle: 2, // 점선
      priceScaleId: 'rsi',
    });
    
    // 과매수/과매도 라인 데이터 설정
    const lineData = rsiData.map(item => ({
      time: item.time
    }));
    
    overboughtLine.setData(lineData.map(item => ({
      ...item,
      value: settings.overbought
    })));
    
    oversoldLine.setData(lineData.map(item => ({
      ...item,
      value: settings.oversold
    })));
    
    return {
      main: rsiSeries,
      overbought: overboughtLine,
      oversold: oversoldLine
    };
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!series) return;
    if (series.main) chart.removeSeries(series.main);
    if (series.overbought) chart.removeSeries(series.overbought);
    if (series.oversold) chart.removeSeries(series.oversold);
  }
};

export default RSI; 