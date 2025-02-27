import React from 'react';
import { calculatePivotPoints } from '../../utils/indicatorCalculations';
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
 * 피봇 포인트(Pivot Points) 지표
 */
const PivotPoints = {
  // 기본 설정
  defaultSettings: {
    // 피봇 포인트는 계산 관련 설정이 필요 없음
    pivotColor: '#673AB7',
    resistanceColor: '#F44336',
    supportColor: '#4CAF50',
    lineWidth: 1
  },
  
  // 설정 UI 컴포넌트
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const [activeTab, setActiveTab] = React.useState('info');
    const [tempSettings, setTempSettings] = React.useState({...settings});
    
    const handleSettingChange = (field, value) => {
      setTempSettings({
        ...tempSettings,
        [field]: field === 'lineWidth' ? parseInt(value) : value
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
            onClick={() => setTempSettings({...PivotPoints.defaultSettings})}
            style={{ padding: '5px 10px', background: 'none', border: 'none', color: '#666', fontSize: '12px', cursor: 'pointer' }}
          >
            초기화
          </button>
        </div>
        
        {/* 탭 선택 버튼 */}
        <div style={{ display: 'flex', borderBottom: '1px solid #ddd', marginBottom: '15px' }}>
          <button
            onClick={() => setActiveTab('info')}
            style={{
              flex: 1,
              padding: '8px 0',
              background: activeTab === 'info' ? '#f5f5f5' : 'transparent',
              border: 'none',
              borderBottom: activeTab === 'info' ? '2px solid #2962FF' : 'none',
              cursor: 'pointer',
              fontWeight: activeTab === 'info' ? 'bold' : 'normal',
            }}
          >
            정보
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
        
        {/* 정보 탭 */}
        {activeTab === 'info' && (
          <div style={{ padding: '10px 0', fontSize: '14px', color: '#666', lineHeight: '1.5' }}>
            <p>피봇 포인트는 계산 관련 설정 옵션이 없습니다.</p>
            <p>가장 최근 데이터를 기준으로 계산됩니다.</p>
          </div>
        )}
        
        {/* 스타일 탭 */}
        {activeTab === 'style' && (
          <>
            <SettingRow>
              <SettingLabel>피봇 라인(PP) 색상</SettingLabel>
              <input
                type="color"
                value={tempSettings.pivotColor}
                onChange={(e) => handleSettingChange('pivotColor', e.target.value)}
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
              <SettingLabel>저항선(R1, R2, R3) 색상</SettingLabel>
              <input
                type="color"
                value={tempSettings.resistanceColor}
                onChange={(e) => handleSettingChange('resistanceColor', e.target.value)}
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
              <SettingLabel>지지선(S1, S2, S3) 색상</SettingLabel>
              <input
                type="color"
                value={tempSettings.supportColor}
                onChange={(e) => handleSettingChange('supportColor', e.target.value)}
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
              <SettingLabel>선 두께</SettingLabel>
              <select
                value={tempSettings.lineWidth}
                onChange={(e) => handleSettingChange('lineWidth', e.target.value)}
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
    return calculatePivotPoints(data);
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data, settings) => {
    const pivotData = PivotPoints.calculate(data, settings);
    const series = {};
    
    // 피봇 포인트 (PP)
    const pivotSeries = chart.addLineSeries({
      color: settings.pivotColor || '#673AB7',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 0, // 실선
      title: 'PP',
    });
    pivotSeries.setData(pivotData.pivot);
    series.pivot = pivotSeries;
    
    // 저항선 1 (R1)
    const r1Series = chart.addLineSeries({
      color: settings.resistanceColor || '#F44336',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'R1',
    });
    r1Series.setData(pivotData.r1);
    series.r1 = r1Series;
    
    // 저항선 2 (R2)
    const r2Series = chart.addLineSeries({
      color: settings.resistanceColor || '#F44336',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'R2',
    });
    r2Series.setData(pivotData.r2);
    series.r2 = r2Series;
    
    // 저항선 3 (R3)
    const r3Series = chart.addLineSeries({
      color: settings.resistanceColor || '#F44336',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'R3',
    });
    r3Series.setData(pivotData.r3);
    series.r3 = r3Series;
    
    // 지지선 1 (S1)
    const s1Series = chart.addLineSeries({
      color: settings.supportColor || '#4CAF50',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'S1',
    });
    s1Series.setData(pivotData.s1);
    series.s1 = s1Series;
    
    // 지지선 2 (S2)
    const s2Series = chart.addLineSeries({
      color: settings.supportColor || '#4CAF50',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'S2',
    });
    s2Series.setData(pivotData.s2);
    series.s2 = s2Series;
    
    // 지지선 3 (S3)
    const s3Series = chart.addLineSeries({
      color: settings.supportColor || '#4CAF50',
      lineWidth: settings.lineWidth || 1,
      lineStyle: 2, // 점선
      title: 'S3',
    });
    s3Series.setData(pivotData.s3);
    series.s3 = s3Series;
    
    return series;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      if (series.pivot && chart) {
        chart.removeSeries(series.pivot);
      }
      
      if (series.r1 && chart) {
        chart.removeSeries(series.r1);
      }
      
      if (series.r2 && chart) {
        chart.removeSeries(series.r2);
      }
      
      if (series.r3 && chart) {
        chart.removeSeries(series.r3);
      }
      
      if (series.s1 && chart) {
        chart.removeSeries(series.s1);
      }
      
      if (series.s2 && chart) {
        chart.removeSeries(series.s2);
      }
      
      if (series.s3 && chart) {
        chart.removeSeries(series.s3);
      }
    } catch (error) {
      console.warn('피봇 포인트 시리즈 제거 중 오류 발생:', error);
    }
  }
};

export default PivotPoints; 