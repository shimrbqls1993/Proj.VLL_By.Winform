// 모든 지표를 등록하고 내보내는 파일
import MA from './chartIndicators/MA';
import Volume from './auxiliaryIndicators/Volume';
import MACD from './auxiliaryIndicators/MACD';
import RSI from './auxiliaryIndicators/RSI';
// 새로 추가한 지표들
import BollingerBands from './chartIndicators/BollingerBands';
import ParabolicSAR from './chartIndicators/ParabolicSAR';
import Envelope from './chartIndicators/Envelope';
import PivotPoints from './chartIndicators/PivotPoints';
import PriceChannel from './chartIndicators/PriceChannel';
import Stochastic from './auxiliaryIndicators/Stochastic';

// 지표 정보 및 UI 설정
export const indicatorsList = [
  { id: 'ma', name: '이동평균선', category: '차트지표', hasSettings: true, component: MA },
  { id: 'bollinger', name: '볼린저밴드', category: '차트지표', hasSettings: true, component: BollingerBands },
  { id: 'parabolicSAR', name: 'Parabolic SAR', category: '차트지표', hasSettings: true, component: ParabolicSAR },
  { id: 'envelope', name: 'Envelope', category: '차트지표', hasSettings: true, component: Envelope },
  { id: 'pivotPoints', name: 'Pivot Points', category: '차트지표', hasSettings: false, component: PivotPoints },
  { id: 'priceChannel', name: 'Price Channel', category: '차트지표', hasSettings: true, component: PriceChannel },
  { id: 'volume', name: '거래량', category: '보조지표', hasSettings: false, component: Volume },
  { id: 'macd', name: 'MACD', category: '보조지표', hasSettings: true, component: MACD },
  { id: 'rsi', name: 'RSI', category: '보조지표', hasSettings: true, component: RSI },
  { id: 'stochastic', name: 'Stochastic', category: '보조지표', hasSettings: true, component: Stochastic },
];

// 카테고리별 지표 분류
export const categorizedIndicators = {
  '차트지표': indicatorsList.filter(i => i.category === '차트지표'),
  '보조지표': indicatorsList.filter(i => i.category === '보조지표'),
};

// 모든 지표 컴포넌트 내보내기
export { 
  MA, 
  Volume, 
  MACD, 
  RSI, 
  BollingerBands, 
  ParabolicSAR, 
  Envelope, 
  PivotPoints, 
  PriceChannel, 
  Stochastic 
}; 