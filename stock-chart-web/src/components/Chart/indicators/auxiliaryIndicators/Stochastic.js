import React from 'react';
import { calculateStochastic } from '../../utils/indicatorCalculations';
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
 * 스토캐스틱(Stochastic) 지표
 */
const Stochastic = {
  // 기본 설정
  defaultSettings: {
    kPeriod: 14,
    dPeriod: 3,
    slowing: 3,
    overbought: 80,
    oversold: 20
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
            onClick={() => onSettingsChange({...Stochastic.defaultSettings})}
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
              <SettingLabel>%K 기간</SettingLabel>
              <SettingInput
                type="number"
                value={settings.kPeriod}
                onChange={(e) => handleSettingChange('kPeriod', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>%D 기간</SettingLabel>
              <SettingInput
                type="number"
                value={settings.dPeriod}
                onChange={(e) => handleSettingChange('dPeriod', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>슬로잉</SettingLabel>
              <SettingInput
                type="number"
                value={settings.slowing}
                onChange={(e) => handleSettingChange('slowing', e.target.value)}
                min="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>과매수 수준</SettingLabel>
              <SettingInput
                type="number"
                value={settings.overbought}
                onChange={(e) => handleSettingChange('overbought', e.target.value)}
                min="50"
                max="100"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>과매도 수준</SettingLabel>
              <SettingInput
                type="number"
                value={settings.oversold}
                onChange={(e) => handleSettingChange('oversold', e.target.value)}
                min="0"
                max="50"
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
    return calculateStochastic(data, settings.kPeriod, settings.dPeriod, settings.slowing);
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings) => {
    const { kLine, dLine } = Stochastic.calculate(data, settings);
    const series = {};
    
    // 새로운 차트 생성 (메인 차트 아래에 위치)
    const stochasticChart = chart.addLineSeries({
      color: '#2962FF',
      lineWidth: 2,
      title: `%K(${settings.kPeriod}, ${settings.dPeriod}, ${settings.slowing})`,
      pane: 1, // 새로운 창에 표시
      priceFormat: {
        type: 'price',
        precision: 2,
        minMove: 0.01,
      },
      scaleMargins: {
        top: 0.1,
        bottom: 0.1,
      },
    });
    stochasticChart.setData(kLine);
    series.kLine = stochasticChart;
    
    // %D 라인 추가
    const dLineSeries = chart.addLineSeries({
      color: '#FF9800',
      lineWidth: 2,
      title: '%D',
      pane: 1, // 같은 창에 표시
      priceFormat: {
        type: 'price',
        precision: 2,
        minMove: 0.01,
      },
    });
    dLineSeries.setData(dLine);
    series.dLine = dLineSeries;
    
    // 과매수 라인 추가
    const overboughtLine = chart.addLineSeries({
      color: 'rgba(255, 82, 82, 0.5)',
      lineWidth: 1,
      lineStyle: 2, // 점선
      title: 'Overbought',
      pane: 1, // 같은 창에 표시
      priceFormat: {
        type: 'price',
        precision: 2,
        minMove: 0.01,
      },
    });
    
    // 과매도 라인 추가
    const oversoldLine = chart.addLineSeries({
      color: 'rgba(76, 175, 80, 0.5)',
      lineWidth: 1,
      lineStyle: 2, // 점선
      title: 'Oversold',
      pane: 1, // 같은 창에 표시
      priceFormat: {
        type: 'price',
        precision: 2,
        minMove: 0.01,
      },
    });
    
    // 과매수/과매도 라인 데이터 생성
    const overboughtData = kLine.map(point => ({
      time: point.time,
      value: settings.overbought
    }));
    
    const oversoldData = kLine.map(point => ({
      time: point.time,
      value: settings.oversold
    }));
    
    overboughtLine.setData(overboughtData);
    oversoldLine.setData(oversoldData);
    
    series.overboughtLine = overboughtLine;
    series.oversoldLine = oversoldLine;
    
    return series;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      if (series.kLine && chart) {
        chart.removeSeries(series.kLine);
      }
      
      if (series.dLine && chart) {
        chart.removeSeries(series.dLine);
      }
      
      if (series.overboughtLine && chart) {
        chart.removeSeries(series.overboughtLine);
      }
      
      if (series.oversoldLine && chart) {
        chart.removeSeries(series.oversoldLine);
      }
    } catch (error) {
      console.warn('스토캐스틱 시리즈 제거 중 오류 발생:', error);
    }
  }
};

export default Stochastic; 