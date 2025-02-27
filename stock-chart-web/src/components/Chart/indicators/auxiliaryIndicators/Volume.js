import React from 'react';

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
    return (
      <div className="volume-settings">
        <p>거래량 지표에는 현재 설정 옵션이 없습니다.</p>
      </div>
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
    chart.removeSeries(series);
  }
};

export default Volume; 