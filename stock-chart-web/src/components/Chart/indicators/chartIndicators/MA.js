import React, { useState } from 'react';
import { calculateSMA, calculateWMA, calculateEMA } from '../../utils/indicatorCalculations';
import { 
  SettingsContainer, 
  SettingTitle, 
  SettingRow, 
  SettingLabel, 
  SettingInput,
  SettingCheckbox,
  SettingButton,
  ButtonGroup
} from '../styles';

/**
 * 이동평균선(MA) 지표
 */
const MA = {
  // 기본 설정
  defaultSettings: {
    weightType: 'simple', // 'simple', 'weighted', 'exponential'
    ma5: { period: 5, enabled: true, lineWidth: 1, color: '#2962FF' },
    ma10: { period: 10, enabled: true, lineWidth: 1, color: '#00C853' },
    ma20: { period: 20, enabled: true, lineWidth: 1, color: '#FF9800' },
    ma60: { period: 60, enabled: true, lineWidth: 1, color: '#E91E63' },
    ma120: { period: 120, enabled: false, lineWidth: 1, color: '#9C27B0' }
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const [newPeriod, setNewPeriod] = useState('');
    const [activeTab, setActiveTab] = useState('variables'); // 'variables' 또는 'style'
    
    // 기본 색상 배열
    const defaultColors = [
      '#2962FF', // ma5
      '#00C853', // ma10
      '#FF9800', // ma20
      '#E91E63', // ma60
      '#9C27B0', // ma120
      '#00BCD4', // 추가 색상
      '#8BC34A',
      '#FFC107',
      '#795548',
      '#607D8B',
      '#3F51B5',
      '#009688'
    ];
    
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
          [field]: field === 'period' || field === 'lineWidth' ? parseInt(value) : value
        }
      });
    };
    
    const handleAddMA = () => {
      if (!newPeriod || isNaN(parseInt(newPeriod)) || parseInt(newPeriod) <= 0) {
        alert('유효한 기간을 입력해주세요.');
        return;
      }
      
      const period = parseInt(newPeriod);
      const maKey = `ma${period}`;
      
      // 이미 존재하는 기간인지 확인
      if (settings[maKey]) {
        alert('이미 존재하는 기간입니다.');
        return;
      }
      
      // 새로운 색상 선택 (기존 색상과 겹치지 않게)
      const existingPeriods = [5, 10, 20, 60, 120];
      const colorIndex = existingPeriods.includes(period) 
        ? existingPeriods.indexOf(period) 
        : 5 + (Object.keys(settings).filter(k => k !== 'weightType').length % (defaultColors.length - 5));
      
      // 새로운 이동평균선 추가
      onSettingsChange({
        ...settings,
        [maKey]: { 
          period, 
          enabled: true, 
          lineWidth: 1, 
          color: defaultColors[colorIndex] 
        }
      });
      
      // 입력 필드 초기화
      setNewPeriod('');
    };
    
    const handleRemoveMA = (maKey) => {
      const newSettings = { ...settings };
      delete newSettings[maKey];
      onSettingsChange(newSettings);
    };

    // 정렬된 MA 설정 배열 가져오기
    const sortedMASettings = Object.entries(settings)
      .filter(([key]) => key !== 'weightType')
      .sort(([keyA], [keyB]) => {
        const periodA = parseInt(keyA.replace('ma', ''));
        const periodB = parseInt(keyB.replace('ma', ''));
        return periodA - periodB;
      });

    return (
      <SettingsContainer>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '5px' }}>
          <button 
            className="reset-button"
            onClick={() => onSettingsChange({...MA.defaultSettings})}
            style={{ padding: '5px 10px', background: 'none', border: 'none', color: '#666', fontSize: '12px', cursor: 'pointer' }}
          >
            초기화
          </button>
        </div>
        
        {/* 탭 선택 버튼 */}
        <div style={{ display: 'flex', borderBottom: '1px solid #ddd', marginBottom: '15px' }}>
          <button
            onClick={() => setActiveTab('variables')}
            style={{
              flex: 1,
              padding: '8px 0',
              background: activeTab === 'variables' ? '#f5f5f5' : 'transparent',
              border: 'none',
              borderBottom: activeTab === 'variables' ? '2px solid #2962FF' : 'none',
              cursor: 'pointer',
              fontWeight: activeTab === 'variables' ? 'bold' : 'normal',
            }}
          >
            변수
          </button>
          <button
            onClick={() => setActiveTab('style')}
            style={{
              flex: 1,
              padding: '8px 0',
              background: activeTab === 'style' ? '#f5f5f5' : 'transparent',
              border: 'none',
              borderBottom: activeTab === 'style' ? '2px solid #2962FF' : 'none',
              cursor: 'pointer',
              fontWeight: activeTab === 'style' ? 'bold' : 'normal',
            }}
          >
            스타일
          </button>
        </div>
        
        {/* 변수 탭 */}
        {activeTab === 'variables' && (
          <>
            <SettingLabel style={{ marginBottom: '5px' }}>가중치 선택</SettingLabel>
            <ButtonGroup>
              <SettingButton
                active={settings.weightType === 'simple'}
                onClick={() => handleWeightTypeChange('simple')}
              >
                단순
              </SettingButton>
              <SettingButton
                active={settings.weightType === 'weighted'}
                onClick={() => handleWeightTypeChange('weighted')}
              >
                가중
              </SettingButton>
              <SettingButton
                active={settings.weightType === 'exponential'}
                onClick={() => handleWeightTypeChange('exponential')}
              >
                지수
              </SettingButton>
            </ButtonGroup>
            
            {sortedMASettings.map(([key, setting]) => (
              <SettingRow key={key}>
                <SettingLabel>
                  <SettingCheckbox
                    checked={setting.enabled}
                    onChange={(e) => handleMaSettingChange(key, 'enabled', e.target.checked)}
                  />
                  {key.replace('ma', '')}일선
                </SettingLabel>
                <div style={{ display: 'flex', alignItems: 'center' }}>
                  <SettingInput
                    type="number"
                    value={setting.period}
                    onChange={(e) => handleMaSettingChange(key, 'period', e.target.value)}
                    min="1"
                    style={{ textAlign: 'left', width: '60px' }}
                  />
                  <button
                    onClick={() => handleRemoveMA(key)}
                    style={{
                      background: 'none',
                      border: 'none',
                      color: '#F44336',
                      cursor: 'pointer',
                      fontSize: '16px',
                      marginLeft: '5px'
                    }}
                  >
                    ×
                  </button>
                </div>
              </SettingRow>
            ))}
            
            <div style={{ marginTop: '15px', borderTop: '1px solid #eee', paddingTop: '15px' }}>
              <SettingLabel>기간 추가</SettingLabel>
              <div style={{ display: 'flex', marginTop: '5px' }}>
                <SettingInput
                  type="number"
                  value={newPeriod}
                  onChange={(e) => setNewPeriod(e.target.value)}
                  placeholder="기간 입력"
                  min="1"
                  style={{ textAlign: 'left', flex: 1 }}
                />
                <button
                  onClick={handleAddMA}
                  style={{
                    background: '#2962FF',
                    color: 'white',
                    border: 'none',
                    borderRadius: '4px',
                    padding: '0 10px',
                    marginLeft: '5px',
                    cursor: 'pointer'
                  }}
                >
                  추가
                </button>
              </div>
            </div>
          </>
        )}
        
        {/* 스타일 탭 */}
        {activeTab === 'style' && (
          <>
            {sortedMASettings.map(([key, setting]) => (
              <div key={key} style={{ marginBottom: '15px', borderBottom: '1px solid #f5f5f5', paddingBottom: '10px' }}>
                <SettingRow>
                  <SettingLabel>
                    <SettingCheckbox
                      checked={setting.enabled}
                      onChange={(e) => handleMaSettingChange(key, 'enabled', e.target.checked)}
                    />
                    {key.replace('ma', '')}일선
                  </SettingLabel>
                  <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                    <input
                      type="color"
                      value={setting.color}
                      onChange={(e) => handleMaSettingChange(key, 'color', e.target.value)}
                      style={{
                        width: '24px',
                        height: '24px',
                        padding: '0',
                        border: '1px solid #ddd',
                        borderRadius: '4px',
                        cursor: 'pointer'
                      }}
                    />
                    <select
                      value={setting.lineWidth}
                      onChange={(e) => handleMaSettingChange(key, 'lineWidth', e.target.value)}
                      style={{
                        padding: '2px 5px',
                        border: '1px solid #ddd',
                        borderRadius: '4px',
                        fontSize: '12px',
                        width: '50px'
                      }}
                    >
                      <option value="1">1pt</option>
                      <option value="2">2pt</option>
                      <option value="3">3pt</option>
                      <option value="4">4pt</option>
                    </select>
                  </div>
                </SettingRow>
              </div>
            ))}
          </>
        )}
      </SettingsContainer>
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
      const setting = settings[key];
      
      const lineSeries = chart.addLineSeries({
        color: setting.color,
        lineWidth: setting.lineWidth,
        title: `MA${setting.period}`,
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