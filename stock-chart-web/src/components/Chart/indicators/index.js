// 모든 지표를 등록하고 내보내는 파일
import MA from './chartIndicators/MA';
import Volume from './auxiliaryIndicators/Volume';
import MACD from './auxiliaryIndicators/MACD';
import RSI from './auxiliaryIndicators/RSI';
// 추가 지표들은 필요에 따라 import

// 지표 정보 및 UI 설정
export const indicatorsList = [
  { 
    id: 'ma', 
    name: '이동평균선', 
    category: '차트지표',
    hasSettings: true,
    component: MA
  },
  { id: 'volume', name: '거래량', category: '보조지표', component: Volume },
  { id: 'macd', name: 'MACD', category: '보조지표', component: MACD },
  { id: 'rsi', name: 'RSI', category: '보조지표', component: RSI },
  // 아직 구현되지 않은 지표들은 주석 처리
  // { id: 'stochastic', name: 'Stochastic', category: '보조지표' },
  // { id: 'bollinger', name: '볼린저밴드', category: '차트지표' },
  // { id: 'parabolicSAR', name: 'Parabolic SAR', category: '차트지표' },
  // { id: 'envelope', name: 'Envelope', category: '차트지표' },
  // { id: 'pivotPoints', name: 'Pivot Points', category: '차트지표' },
  // { id: 'priceChannel', name: 'Price Channel', category: '차트지표' }
];

// 카테고리별 지표 분류
export const categorizedIndicators = {
  '차트지표': indicatorsList.filter(i => i.category === '차트지표'),
  '보조지표': indicatorsList.filter(i => i.category === '보조지표'),
};

// 모든 지표 컴포넌트 내보내기
export { MA, Volume, MACD, RSI }; 