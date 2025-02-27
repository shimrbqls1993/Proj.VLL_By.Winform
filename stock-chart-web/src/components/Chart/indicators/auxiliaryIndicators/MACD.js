import React from 'react';
import { calculateMACD } from '../../utils/indicatorCalculations';

/**
 * MACD(Moving Average Convergence Divergence) 지표
 */
const MACD = {
  // 기본 설정
  defaultSettings: {
    shortPeriod: 12,
    longPeriod: 26,
    signalPeriod: 9
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
      <div className="macd-settings">
        <div className="setting-group">
          <label>
            단기 EMA 기간:
            <input
              type="number"
              value={settings.shortPeriod}
              onChange={(e) => handleSettingChange('shortPeriod', e.target.value)}
              min="1"
            />
          </label>
        </div>
        <div className="setting-group">
          <label>
            장기 EMA 기간:
            <input
              type="number"
              value={settings.longPeriod}
              onChange={(e) => handleSettingChange('longPeriod', e.target.value)}
              min="1"
            />
          </label>
        </div>
        <div className="setting-group">
          <label>
            시그널 라인 기간:
            <input
              type="number"
              value={settings.signalPeriod}
              onChange={(e) => handleSettingChange('signalPeriod', e.target.value)}
              min="1"
            />
          </label>
        </div>
      </div>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data, settings) => {
    return calculateMACD(
      data, 
      settings.shortPeriod, 
      settings.longPeriod, 
      settings.signalPeriod
    );
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings = MACD.defaultSettings) => {
    const macdData = MACD.calculate(data, settings);
    
    const macdSeries = chart.addLineSeries({
      color: '#2196F3',
      lineWidth: 1,
      title: 'MACD',
      priceScaleId: 'macd',
      scaleMargins: {
        top: 0.7,
        bottom: 0.2,
      },
    });
    
    macdSeries.setData(macdData);
    
    return macdSeries;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    chart.removeSeries(series);
  }
};

export default MACD; 