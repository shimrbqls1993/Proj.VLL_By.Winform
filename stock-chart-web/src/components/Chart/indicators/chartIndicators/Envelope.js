import React from 'react';
import { calculateEnvelope } from '../../utils/indicatorCalculations';
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
 * 엔벨로프(Envelope) 지표
 */
const Envelope = {
  // 기본 설정
  defaultSettings: {
    period: 20,
    upperPercentage: 0.1,
    lowerPercentage: 0.1,
    color: '#FF9800',
    middleLineWidth: 1,
    bandLineWidth: 1,
    showBackground: true
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const [activeTab, setActiveTab] = React.useState('variables');
    const [tempSettings, setTempSettings] = React.useState({...settings});
    
    const handleSettingChange = (field, value) => {
      setTempSettings({
        ...tempSettings,
        [field]: field === 'period' ? parseInt(value) : 
                 field === 'middleLineWidth' || field === 'bandLineWidth' ? parseInt(value) : 
                 field === 'showBackground' ? value : 
                 parseFloat(value)
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
            onClick={() => setTempSettings({...Envelope.defaultSettings})}
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
              <SettingLabel>기간</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.period}
                onChange={(e) => handleSettingChange('period', e.target.value)}
                min="5"
                max="240"
                step="1"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>상단 폭(%)</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.upperPercentage}
                onChange={(e) => handleSettingChange('upperPercentage', e.target.value)}
                min="0.01"
                max="10"
                step="0.01"
                style={{ textAlign: 'left', width: '60px' }}
              />
            </SettingRow>
            
            <SettingRow>
              <SettingLabel>하단 폭(%)</SettingLabel>
              <SettingInput
                type="number"
                value={tempSettings.lowerPercentage}
                onChange={(e) => handleSettingChange('lowerPercentage', e.target.value)}
                min="0.01"
                max="10"
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
              <SettingLabel>중간선 두께</SettingLabel>
              <select
                value={tempSettings.middleLineWidth}
                onChange={(e) => handleSettingChange('middleLineWidth', e.target.value)}
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
            
            <SettingRow>
              <SettingLabel>밴드선 두께</SettingLabel>
              <select
                value={tempSettings.bandLineWidth}
                onChange={(e) => handleSettingChange('bandLineWidth', e.target.value)}
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
            
            <SettingRow>
              <SettingLabel>배경 표시</SettingLabel>
              <SettingCheckbox
                checked={tempSettings.showBackground}
                onChange={(e) => handleSettingChange('showBackground', e.target.checked)}
              />
            </SettingRow>
          </>
        )}
      </SettingsContainer>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data, settings) => {
    return calculateEnvelope(data, settings.period, settings.upperPercentage, settings.lowerPercentage);
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings) => {
    const { middle, upper, lower } = Envelope.calculate(data, settings);
    const series = {};
    
    // 중간선 (SMA)
    const middleSeries = chart.addLineSeries({
      color: settings.color || '#FF9800',
      lineWidth: settings.middleLineWidth || 1,
      title: `ENV(${settings.period})`,
    });
    middleSeries.setData(middle);
    series.middle = middleSeries;
    
    // 상단선
    const upperSeries = chart.addLineSeries({
      color: settings.color || '#FF9800',
      lineWidth: settings.bandLineWidth || 1,
      title: `ENV Upper(${settings.period}, ${settings.upperPercentage}%)`,
    });
    upperSeries.setData(upper);
    series.upper = upperSeries;
    
    // 하단선
    const lowerSeries = chart.addLineSeries({
      color: settings.color || '#FF9800',
      lineWidth: settings.bandLineWidth || 1,
      title: `ENV Lower(${settings.period}, ${settings.lowerPercentage}%)`,
    });
    lowerSeries.setData(lower);
    series.lower = lowerSeries;
    
    // 배경색 표시 옵션에 따라 영역 추가
    if (settings.showBackground !== false) {
      // 밴드 사이의 영역을 채우기 위한 시리즈
      const rangeSeries = chart.addCandlestickSeries({
        upColor: 'transparent',
        downColor: 'transparent',
        borderVisible: false,
        wickUpColor: 'transparent',
        wickDownColor: 'transparent',
        priceFormat: {
          type: 'price',
          precision: 0,
          minMove: 1,
        },
      });
      
      // 영역 데이터 생성 (상단선과 하단선 사이)
      const rangeData = [];
      for (let i = 0; i < upper.length; i++) {
        if (upper[i] && lower[i]) {
          rangeData.push({
            time: upper[i].time,
            open: upper[i].value,
            high: upper[i].value,
            low: lower[i].value,
            close: lower[i].value
          });
        }
      }
      
      // 영역 색상 설정
      const colorWithOpacity = `${settings.color || '#FF9800'}20`;
      rangeSeries.applyOptions({
        // 캔들 내부 색상 설정
        upColor: colorWithOpacity,
        downColor: colorWithOpacity,
      });
      
      rangeSeries.setData(rangeData);
      series.range = rangeSeries;
    }
    
    return series;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      if (series.range && chart) {
        chart.removeSeries(series.range);
      }
      
      if (series.middle && chart) {
        chart.removeSeries(series.middle);
      }
      
      if (series.upper && chart) {
        chart.removeSeries(series.upper);
      }
      
      if (series.lower && chart) {
        chart.removeSeries(series.lower);
      }
    } catch (error) {
      console.warn('엔벨로프 시리즈 제거 중 오류 발생:', error);
    }
  }
};

export default Envelope; 