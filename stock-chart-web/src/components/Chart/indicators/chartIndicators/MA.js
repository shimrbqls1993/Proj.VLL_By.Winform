import React from 'react';
import { calculateSMA, calculateWMA, calculateEMA } from '../../utils/indicatorCalculations';

/**
 * 이동평균선(MA) 지표
 */
const MA = {
  // 기본 설정
  defaultSettings: {
    weightType: 'simple', // 'simple', 'weighted', 'exponential'
    ma5: { period: 5, enabled: true },
    ma10: { period: 10, enabled: true },
    ma20: { period: 20, enabled: true },
    ma60: { period: 60, enabled: true },
    ma120: { period: 120, enabled: false }
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const handleWeightTypeChange = (type) => {
      onSettingsChange({
        ...settings,
        weightType: type
      });
    };

    const handleMaSettingChange = (maKey, field, value) => {
      onSettingsChange({
        ...settings,
        [maKey]: {
          ...settings[maKey],
          [field]: field === 'period' ? parseInt(value) : value
        }
      });
    };

    return (
      <div className="ma-settings">
        <div className="weight-type-selector">
          <h5>가중치 선택</h5>
          <div className="weight-type-buttons">
            <button
              className={settings.weightType === 'simple' ? 'active' : ''}
              onClick={() => handleWeightTypeChange('simple')}
            >
              단순
            </button>
            <button
              className={settings.weightType === 'weighted' ? 'active' : ''}
              onClick={() => handleWeightTypeChange('weighted')}
            >
              가중
            </button>
            <button
              className={settings.weightType === 'exponential' ? 'active' : ''}
              onClick={() => handleWeightTypeChange('exponential')}
            >
              지수
            </button>
          </div>
        </div>
        <div className="ma-settings-list">
          {Object.entries(settings).map(([key, setting]) => {
            if (key === 'weightType') return null;
            return (
              <div key={key} className="ma-setting-group">
                <label>
                  <input
                    type="checkbox"
                    checked={setting.enabled}
                    onChange={(e) => handleMaSettingChange(key, 'enabled', e.target.checked)}
                  />
                  <input
                    type="number"
                    value={setting.period}
                    onChange={(e) => handleMaSettingChange(key, 'period', e.target.value)}
                    min="1"
                  />
                  일선
                </label>
              </div>
            );
          })}
        </div>
      </div>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data, settings) => {
    const result = {};
    
    // 각 이동평균선 계산
    Object.entries(settings).forEach(([key, setting]) => {
      if (key !== 'weightType' && setting.enabled) {
        let maData;
        
        if (settings.weightType === 'simple') {
          maData = calculateSMA(data, setting.period);
        } else if (settings.weightType === 'weighted') {
          maData = calculateWMA(data, setting.period);
        } else if (settings.weightType === 'exponential') {
          maData = calculateEMA(data, setting.period);
        }
        
        result[key] = maData;
      }
    });
    
    return result;
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings) => {
    const calculatedData = MA.calculate(data, settings);
    const series = {};
    
    // 각 이동평균선을 차트에 추가
    Object.entries(calculatedData).forEach(([key, lineData]) => {
      const color = key === 'ma5' ? '#2962FF' :
                   key === 'ma10' ? '#00C853' :
                   key === 'ma20' ? '#FF9800' :
                   key === 'ma60' ? '#E91E63' :
                   '#9C27B0'; // ma120
                   
      const lineSeries = chart.addLineSeries({
        color: color,
        lineWidth: 1,
        title: `MA${settings[key].period}`,
      });
      
      lineSeries.setData(lineData);
      series[key] = lineSeries;
    });
    
    return series;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      Object.entries(series).forEach(([key, s]) => {
        if (s && chart) {
          try {
            chart.removeSeries(s);
          } catch (error) {
            console.warn(`MA 시리즈 제거 오류 (${key}):`, error);
          }
        }
      });
    } catch (error) {
      console.warn('MA 시리즈 제거 중 오류 발생:', error);
    }
  }
};

export default MA; 