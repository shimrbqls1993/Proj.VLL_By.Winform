import React from 'react';
import { calculateMACD } from '../../utils/indicatorCalculations';
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
    const [activeTab, setActiveTab] = React.useState('variables');
    
    const handleSettingChange = (field, value) => {
      onSettingsChange({
        ...settings,
        [field]: parseInt(value)
      });
    };

    return (
      <SettingsContainer>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '5px' }}>
          <button 
            className="reset-button"
            onClick={() => onSettingsChange({...MACD.defaultSettings})}
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
        </div>
        
        {/* 변수 탭 */}
        {activeTab === 'variables' && (
          <>
            <SettingRow>
              <SettingLabel>단기 EMA 기간</SettingLabel>
              <SettingInput
                type="number"
                value={settings.shortPeriod}
                onChange={(e) => handleSettingChange('shortPeriod', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>장기 EMA 기간</SettingLabel>
              <SettingInput
                type="number"
                value={settings.longPeriod}
                onChange={(e) => handleSettingChange('longPeriod', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>시그널 라인 기간</SettingLabel>
              <SettingInput
                type="number"
                value={settings.signalPeriod}
                onChange={(e) => handleSettingChange('signalPeriod', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
          </>
        )}
      </SettingsContainer>
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
    
    try {
      chart.removeSeries(series);
    } catch (error) {
      console.warn('MACD 시리즈 제거 오류:', error);
    }
  }
};

export default MACD; 