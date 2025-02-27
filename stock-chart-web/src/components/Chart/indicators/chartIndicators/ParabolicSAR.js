import React from 'react';
import { calculateParabolicSAR } from '../../utils/indicatorCalculations';
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
 * 파라볼릭 SAR(Parabolic Stop and Reverse) 지표
 */
const ParabolicSAR = {
  // 기본 설정
  defaultSettings: {
    step: 0.02,
    initialStep: 0.02,
    max: 0.2,
    color: '#9C27B0',
    pointSize: 2
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const [activeTab, setActiveTab] = React.useState('variables');
    const [tempSettings, setTempSettings] = React.useState({...settings});
    
    const handleSettingChange = (field, value) => {
      setTempSettings({
        ...tempSettings,
        [field]: field === 'pointSize' ? parseInt(value) : parseFloat(value)
      });
    };

    const handleColorChange = (color) => {
      setTempSettings({
        ...tempSettings,
        color
      });
    };

    // 설정 변경 시 바로 적용
    React.useEffect(() => {
      onSettingsChange(tempSettings);
    }, [tempSettings, onSettingsChange]);

    return (
      <SettingsContainer>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '5px' }}>
          <button 
            className="reset-button"
            onClick={() => setTempSettings({...ParabolicSAR.defaultSettings})}
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
            <SettingRow>
              <SettingLabel>가속증가량</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.step}
                onChange={(e) => handleSettingChange('step', e.target.value)}
                min="0.001"
                max="0.1"
                step="0.001"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>초기가속요소</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.initialStep}
                onChange={(e) => handleSettingChange('initialStep', e.target.value)}
                min="0.001"
                max="0.1"
                step="0.001"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>최대가속요소</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.max}
                onChange={(e) => handleSettingChange('max', e.target.value)}
                min="0.1"
                max="0.5"
                step="0.01"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
          </>
        )}
        
        {/* 스타일 탭 */}
        {activeTab === 'style' && (
          <>
            <SettingRow>
              <SettingLabel>색상</SettingLabel>
              <input
                type="color"
                value={tempSettings.color}
                onChange={(e) => handleColorChange(e.target.value)}
                style={{
                  width: '24px',
                  height: '24px',
                  padding: '0',
                  border: '1px solid #ddd',
                  borderRadius: '4px',
                  cursor: 'pointer'
                }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>점 크기</SettingLabel>
              <select
                value={tempSettings.pointSize}
                onChange={(e) => handleSettingChange('pointSize', e.target.value)}
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
            </SettingRow>
          </>
        )}
      </SettingsContainer>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data, settings) => {
    return calculateParabolicSAR(data, settings.step, settings.max);
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings) => {
    const sarData = ParabolicSAR.calculate(data, settings);
    
    // SAR 포인트 시리즈 추가
    const series = chart.addLineSeries({
      color: settings.color,
      lineWidth: 0,
      pointsVisible: true,
      pointSize: settings.pointSize,
      title: `SAR(${settings.step}, ${settings.max})`,
    });
    
    series.setData(sarData);
    
    return series;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      chart.removeSeries(series);
    } catch (error) {
      console.warn('파라볼릭 SAR 시리즈 제거 중 오류 발생:', error);
    }
  }
};

export default ParabolicSAR; 