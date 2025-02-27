import React from 'react';
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
 * 거래량(Volume) 지표
 */
const Volume = {
  // 기본 설정
  defaultSettings: {
    // 거래량은 현재 특별한 설정이 없음
  },
  
  // 설정 UI 컴포넌트 (현재는 설정이 없으므로 간단한 메시지만 표시)
  SettingsComponent: ({ settings, onSettingsChange }) => {
    const [activeTab, setActiveTab] = React.useState('info');
    
    return (
      <SettingsContainer>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '5px' }}>
          <button 
            className="reset-button"
            onClick={() => onSettingsChange({...Volume.defaultSettings})}
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
        </div>
        
        {/* 정보 탭 */}
        {activeTab === 'info' && (
          <div style={{ padding: '10px 0', fontSize: '14px', color: '#666', lineHeight: '1.5' }}>
            <p>거래량 지표에는 현재 설정 옵션이 없습니다.</p>
          </div>
        )}
      </SettingsContainer>
    );
  },
  
  // 데이터 계산 함수
  calculate: (data) => {
    return data.map(item => ({
      time: item.time,
      value: item.volume,
      color: item.close >= item.open ? '#ff1744' : '#1e88e5'
    }));
  },
  
  // 차트에 시리즈 추가 함수
  addToChart: (chart, data) => {
    const volumeData = Volume.calculate(data);
    
    const volumeSeries = chart.addHistogramSeries({
      color: '#26a69a',
      priceFormat: {
        type: 'volume',
      },
      priceScaleId: 'volume',
    });
    
    volumeSeries.setData(volumeData);
    
    return volumeSeries;
  },
  
  // 차트에서 시리즈 제거 함수
  removeFromChart: (chart, series) => {
    if (!chart || !series) return;
    
    try {
      chart.removeSeries(series);
    } catch (error) {
      console.warn('Volume 시리즈 제거 오류:', error);
    }
  }
};

export default Volume; 