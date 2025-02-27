import React, { useState } from 'react';
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

    React.useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const response = await fetch(
                    `${process.env.REACT_APP_API_URL}/candlesticks/${code}/${timeFrame}`
                );
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setChartData(data);
                setError(null);
            } catch (err) {
                setError('데이터를 불러오는데 실패했습니다.');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [code, timeFrame]);

    if (loading) return <Loading />;
    if (error) return <div className="error-message">{error}</div>;

    return (
        <div className="chart-container">
            <div className="chart-controls">
                <TimeFrameSelector
                    selectedTimeFrame={timeFrame}
                    onTimeFrameChange={setTimeFrame}
                />
                <button 
                    className="timeframe-button indicator-button"
                    onClick={() => setIsIndicatorSidebarOpen(prev => !prev)}
                >
                    지표
                </button>
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
