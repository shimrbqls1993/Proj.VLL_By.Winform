import React, { useState, useEffect } from 'react';
import CandleStickChart from './CandleStickChart';
import TimeFrameSelector from './TimeFrameSelector';
import IndicatorSidebar from './IndicatorSidebar';
import Loading from '../Common/Loading';
import { 
    MA, 
    MACD, 
    RSI, 
    BollingerBands, 
    ParabolicSAR, 
    Envelope, 
    PivotPoints, 
    PriceChannel, 
    Stochastic 
} from './indicators';

const ChartContainer = ({ code }) => {
    const [timeFrame, setTimeFrame] = useState('DAY');
    const [chartData, setChartData] = useState([]);
    const [dailyData, setDailyData] = useState([]); // 일봉 데이터 저장
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isIndicatorSidebarOpen, setIsIndicatorSidebarOpen] = useState(false);
    const [selectedIndicators, setSelectedIndicators] = useState(['volume']); // 기본적으로 거래량 표시
    const [indicatorSettings, setIndicatorSettings] = useState({
        ma: MA.defaultSettings,
        macd: MACD.defaultSettings,
        rsi: RSI.defaultSettings,
        // 새로 추가한 지표들의 기본 설정
        bollinger: BollingerBands.defaultSettings,
        parabolicSAR: ParabolicSAR.defaultSettings,
        envelope: Envelope.defaultSettings,
        pivotPoints: PivotPoints.defaultSettings,
        priceChannel: PriceChannel.defaultSettings,
        stochastic: Stochastic.defaultSettings
    });

    const handleIndicatorSelect = (indicatorId) => {
        setSelectedIndicators(prev => 
            prev.includes(indicatorId)
                ? prev.filter(id => id !== indicatorId)
                : [...prev, indicatorId]
        );
    };

    const handleUpdateSettings = (indicatorId, settings) => {
        console.log(`지표 설정 업데이트: ${indicatorId}`, settings);
        setIndicatorSettings(prev => ({
            ...prev,
            [indicatorId]: settings
        }));
    };

    // 일봉 데이터 가져오기
    useEffect(() => {
        const fetchDailyData = async () => {
            try {
                setLoading(true);
                const response = await fetch(
                    `${process.env.REACT_APP_API_URL}/candlesticks/${code}/DAY`
                );
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setDailyData(data);
                setError(null);
            } catch (err) {
                setError('일봉 데이터를 불러오는데 실패했습니다.');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchDailyData();
    }, [code]);

    // 시간 프레임에 따라 데이터 가져오기 또는 계산하기
    useEffect(() => {
        const fetchOrCalculateData = async () => {
            try {
                setLoading(true);
                
                // 주봉, 월봉, 연봉은 일봉 데이터로 계산
                if (['WEEK', 'MONTH', 'YEAR'].includes(timeFrame) && dailyData.length > 0) {
                    console.log(`${timeFrame} 계산 시작, 일봉 데이터 개수:`, dailyData.length);
                    const calculatedData = calculateAggregatedData(dailyData, timeFrame);
                    console.log(`계산된 ${timeFrame} 데이터:`, calculatedData);
                    setChartData(calculatedData);
                } else {
                    // 다른 시간 프레임은 API에서 가져오기
                    const response = await fetch(
                        `${process.env.REACT_APP_API_URL}/candlesticks/${code}/${timeFrame}`
                    );
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    const data = await response.json();
                    console.log(`API에서 가져온 ${timeFrame} 데이터:`, data);
                    setChartData(data);
                }
                
                setError(null);
            } catch (err) {
                setError('데이터를 불러오는데 실패했습니다.');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchOrCalculateData();
    }, [code, timeFrame, dailyData]);

    // 일봉 데이터를 기반으로 주봉, 월봉, 연봉 계산
    const calculateAggregatedData = (dailyData, timeFrame) => {
        if (!dailyData || dailyData.length === 0) {
            console.log('일봉 데이터가 없습니다.');
            return [];
        }

        // 일봉 데이터의 첫 항목을 확인하여 데이터 형식 파악
        const sampleDailyData = dailyData[0];
        console.log('일봉 데이터 샘플:', sampleDailyData);

        // 타임스탬프 변환 함수 - 초 단위를 밀리초로 변환
        const convertTimestamp = (timestamp) => {
            // 숫자인지 확인하고 길이로 초/밀리초 구분
            if (typeof timestamp === 'number') {
                // 초 단위 타임스탬프는 보통 10자리, 밀리초는 13자리
                return timestamp.toString().length <= 10 ? timestamp * 1000 : timestamp;
            }
            // 문자열이면 그대로 반환
            return timestamp;
        };

        // 날짜 기준으로 정렬 (오래된 날짜부터)
        const sortedData = [...dailyData].sort((a, b) => {
            const timeA = convertTimestamp(a.time);
            const timeB = convertTimestamp(b.time);
            return new Date(timeA) - new Date(timeB);
        });
        
        console.log('정렬된 일봉 데이터 첫 항목:', sortedData[0]);
        console.log('정렬된 일봉 데이터 마지막 항목:', sortedData[sortedData.length - 1]);
        
        // 주, 월, 연 단위로 데이터 그룹화
        const groupedData = {};
        
        if (timeFrame === 'WEEK') {
            // 주봉 계산 (5일 거래일 기준)
            let weekCounter = 0;
            let currentWeekData = null;
            
            sortedData.forEach((candle, index) => {
                // 새로운 주의 시작 또는 첫 데이터
                if (weekCounter % 5 === 0 || index === 0) {
                    // 이전 주 데이터가 있으면 저장
                    if (currentWeekData) {
                        const weekKey = `week-${Math.floor(weekCounter / 5)}`;
                        groupedData[weekKey] = currentWeekData;
                    }
                    
                    // 새 주 시작
                    currentWeekData = {
                        time: candle.time,
                        open: candle.open,
                        high: candle.high,
                        low: candle.low,
                        close: candle.close,
                        volume: candle.volume
                    };
                } else {
                    // 기존 주 데이터 업데이트
                    currentWeekData.high = Math.max(currentWeekData.high, candle.high);
                    currentWeekData.low = Math.min(currentWeekData.low, candle.low);
                    currentWeekData.close = candle.close;
                    currentWeekData.volume += candle.volume;
                    // 시간은 가장 최근 시간으로 업데이트
                    const currentTime = convertTimestamp(currentWeekData.time);
                    const candleTime = convertTimestamp(candle.time);
                    if (new Date(candleTime) > new Date(currentTime)) {
                        currentWeekData.time = candle.time;
                    }
                }
                
                weekCounter++;
            });
            
            // 마지막 주 데이터 저장
            if (currentWeekData) {
                const weekKey = `week-${Math.floor(weekCounter / 5)}`;
                groupedData[weekKey] = currentWeekData;
            }
        } else {
            // 월봉, 연봉 계산을 위한 임시 그룹화
            const tempGroups = {};
            
            // 먼저 데이터를 월 또는 연 단위로 그룹화
            sortedData.forEach(candle => {
                const timestamp = convertTimestamp(candle.time);
                const date = new Date(timestamp);
                let key;
                
                if (timeFrame === 'MONTH') {
                    // 월 기준 그룹화
                    key = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
                } else if (timeFrame === 'YEAR') {
                    // 연 기준 그룹화
                    key = `${date.getFullYear()}`;
                }
                
                if (!tempGroups[key]) {
                    tempGroups[key] = [];
                }
                
                tempGroups[key].push(candle);
            });
            
            console.log(`${timeFrame} 그룹 개수:`, Object.keys(tempGroups).length);
            console.log(`${timeFrame} 그룹 키:`, Object.keys(tempGroups));
            
            // 각 그룹에 대해 시가, 고가, 저가, 종가, 거래량 계산
            Object.keys(tempGroups).forEach(key => {
                const candles = tempGroups[key];
                console.log(`${key} 그룹의 캔들 개수:`, candles.length);
                
                // 날짜순으로 정렬
                candles.sort((a, b) => {
                    const timeA = convertTimestamp(a.time);
                    const timeB = convertTimestamp(b.time);
                    return new Date(timeA) - new Date(timeB);
                });
                
                // 첫 거래일의 시가, 마지막 거래일의 종가
                const firstCandle = candles[0];
                const lastCandle = candles[candles.length - 1];
                
                // 모든 캔들에서 최고가와 최저가 찾기
                let highestHigh = -Infinity;
                let lowestLow = Infinity;
                let totalVolume = 0;
                
                candles.forEach(candle => {
                    highestHigh = Math.max(highestHigh, candle.high);
                    lowestLow = Math.min(lowestLow, candle.low);
                    totalVolume += candle.volume;
                });
                
                // 일봉 데이터와 동일한 형식으로 월봉/연봉 데이터 생성
                const aggregatedCandle = {
                    ...sampleDailyData, // 기본 구조 복사
                    time: lastCandle.time, // 마지막 거래일의 시간
                    open: firstCandle.open, // 첫 거래일의 시가
                    high: highestHigh,
                    low: lowestLow,
                    close: lastCandle.close, // 마지막 거래일의 종가
                    volume: totalVolume
                };
                
                groupedData[key] = aggregatedCandle;
                
                console.log(`${key} 그룹의 계산된 데이터:`, groupedData[key]);
            });
        }
        
        // 객체를 배열로 변환하고 날짜순으로 정렬
        const result = Object.values(groupedData).sort((a, b) => {
            const timeA = convertTimestamp(a.time);
            const timeB = convertTimestamp(b.time);
            return new Date(timeA) - new Date(timeB);
        });
        
        console.log(`최종 계산된 ${timeFrame} 데이터 개수:`, result.length);
        
        // 결과가 비어있으면 원본 일봉 데이터 반환 (디버깅용)
        if (result.length === 0 && dailyData.length > 0) {
            console.log('계산된 데이터가 없어 일봉 데이터를 반환합니다.');
            return dailyData;
        }
        
        return result;
    };

    if (loading) return <Loading />;
    if (error) return <div className="error-message">{error}</div>;

    return (
        <div className="chart-container">
            <div className="chart-controls" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <button 
                    className="timeframe-button indicator-button"
                    onClick={() => setIsIndicatorSidebarOpen(prev => !prev)}
                >
                    <svg 
                        xmlns="http://www.w3.org/2000/svg" 
                        width="16" 
                        height="16" 
                        viewBox="0 0 24 24" 
                        fill="none" 
                        stroke="currentColor" 
                        strokeWidth="2" 
                        strokeLinecap="round" 
                        strokeLinejoin="round" 
                        style={{ marginRight: '5px' }}
                    >
                        <polyline points="22 12 18 12 15 21 9 3 6 12 2 12"></polyline>
                    </svg>
                    지표
                </button>
                <div style={{ flex: 1 }}>
                    <TimeFrameSelector
                        selectedTimeFrame={timeFrame}
                        onTimeFrameChange={setTimeFrame}
                    />
                </div>
            </div>
            <CandleStickChart 
                data={chartData} 
                code={code}
                indicators={selectedIndicators}
                indicatorSettings={indicatorSettings}
            />
            <IndicatorSidebar
                isOpen={isIndicatorSidebarOpen}
                onClose={() => setIsIndicatorSidebarOpen(false)}
                onSelectIndicator={handleIndicatorSelect}
                selectedIndicators={selectedIndicators}
                onUpdateSettings={handleUpdateSettings}
                indicatorSettings={indicatorSettings}
            />
        </div>
    );
};

export default ChartContainer;
