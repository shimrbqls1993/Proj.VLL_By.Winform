import React from 'react';
import { createChart } from 'lightweight-charts';

const CandleStickChart = ({ data, code, indicators, indicatorSettings }) => {
    const mainChartRef = React.useRef();
    const volumeChartRef = React.useRef();
    const mainChart = React.useRef();
    const volumeChart = React.useRef();
    const chartContainerRef = React.useRef();

    React.useEffect(() => {
        if (!data || data.length === 0) return;

        // 메인 차트 생성 (가격)
        mainChart.current = createChart(mainChartRef.current, {
            width: chartContainerRef.current.clientWidth,
            height: 480,  // 메인 차트 높이
            layout: {
                background: { color: '#ffffff' },
                textColor: '#333',
                fontSize: 12,
            },
            grid: {
                vertLines: { 
                    color: 'rgba(220, 220, 220, 0.8)',
                    style: 1,
                },
                horzLines: { 
                    color: 'rgba(220, 220, 220, 0.8)',
                    style: 1,
                },
            },
            crosshair: {
                mode: 1,
                vertLine: {
                    width: 1,
                    color: '#2962FF',
                    style: 0,
                    labelBackgroundColor: '#2962FF',
                },
                horzLine: {
                    width: 1,
                    color: '#2962FF',
                    style: 0,
                    labelBackgroundColor: '#2962FF',
                },
            },
            timeScale: {
                timeVisible: true,
                secondsVisible: false,
                borderColor: '#D1D4DC',
                rightOffset: 12,
                barSpacing: 6,
                fixLeftEdge: true,
                lockVisibleTimeRangeOnResize: true,
                rightBarStaysOnScroll: true,
                borderVisible: true,
                visible: true,
            },
            rightPriceScale: {
                borderColor: '#D1D4DC',
                borderVisible: true,
                scaleMargins: {
                    top: 0.1,
                    bottom: 0.1,
                },
            },
        });

        // 거래량 차트 생성
        volumeChart.current = createChart(volumeChartRef.current, {
            width: chartContainerRef.current.clientWidth,
            height: 120,  // 거래량 차트 높이
            layout: {
                background: { color: '#ffffff' },
                textColor: '#333',
                fontSize: 12,
            },
            grid: {
                vertLines: { 
                    color: 'rgba(220, 220, 220, 0.8)',
                    style: 1,
                },
                horzLines: { 
                    color: 'rgba(220, 220, 220, 0.8)',
                    style: 1,
                },
            },
            timeScale: {
                visible: false,  // 시간축 숨김
            },
            rightPriceScale: {
                borderColor: '#D1D4DC',
                borderVisible: true,
                scaleMargins: {
                    top: 0.1,
                    bottom: 0.1,
                },
            },
        });

        // 메인 차트에 캔들스틱 시리즈 추가
        const candleSeries = mainChart.current.addCandlestickSeries({
            upColor: '#ff1744',
            downColor: '#1e88e5',
            borderVisible: false,
            wickUpColor: '#ff1744',
            wickDownColor: '#1e88e5',
            priceFormat: {
                type: 'price',
                precision: 0,
                minMove: 1,
            },
        });

        candleSeries.setData(data);

        // 거래량 차트에 거래량 시리즈 추가
        if (indicators.includes('volume')) {
            const volumeSeries = volumeChart.current.addHistogramSeries({
                color: '#26a69a',
                priceFormat: {
                    type: 'volume',
                },
                priceScaleId: 'volume',
            });

            volumeSeries.setData(
                data.map(item => ({
                    time: item.time,
                    value: item.volume,
                    color: item.close >= item.open ? '#ff1744' : '#1e88e5'
                }))
            );

            // 데이터 설정 후 차트 동기화 설정
            setTimeout(() => {
                // 메인 차트 동기화
                mainChart.current.timeScale().subscribeVisibleTimeRangeChange(() => {
                    const mainTimeScale = mainChart.current.timeScale();
                    const volumeTimeScale = volumeChart.current.timeScale();
                    const timeRange = mainTimeScale.getVisibleRange();
                    
                    if (timeRange && volumeTimeScale) {
                        try {
                            volumeTimeScale.setVisibleRange(timeRange);
                        } catch (error) {
                            console.log('Sync error:', error);
                        }
                    }
                });

                // 거래량 차트 동기화
                volumeChart.current.timeScale().subscribeVisibleTimeRangeChange(() => {
                    const mainTimeScale = mainChart.current.timeScale();
                    const volumeTimeScale = volumeChart.current.timeScale();
                    const timeRange = volumeTimeScale.getVisibleRange();
                    
                    if (timeRange && mainTimeScale) {
                        try {
                            mainTimeScale.setVisibleRange(timeRange);
                        } catch (error) {
                            console.log('Sync error:', error);
                        }
                    }
                });

                // 초기 시간 범위 동기화
                const initialTimeRange = mainChart.current.timeScale().getVisibleRange();
                if (initialTimeRange) {
                    volumeChart.current.timeScale().setVisibleRange(initialTimeRange);
                }
            }, 100);
        }

        // 이동평균선 추가 (MA가 선택된 경우에만)
        if (indicators.includes('ma')) {
            const maSettings = indicatorSettings.ma;
            
            // 이동평균선 데이터 계산
            const calculateMA = (data, period, weightType = 'simple') => {
                const result = [];
                
                if (weightType === 'simple') {
                    // 단순 이동평균 (SMA)
                    for (let i = 0; i < data.length; i++) {
                        if (i < period - 1) continue;
                        let sum = 0;
                        for (let j = 0; j < period; j++) {
                            sum += data[i - j].close;
                        }
                        result.push({
                            time: data[i].time,
                            value: sum / period
                        });
                    }
                } else if (weightType === 'weighted') {
                    // 가중 이동평균 (WMA)
                    for (let i = period - 1; i < data.length; i++) {
                        let sum = 0;
                        let weightSum = 0;
                        for (let j = 0; j < period; j++) {
                            const weight = period - j;
                            sum += data[i - j].close * weight;
                            weightSum += weight;
                        }
                        result.push({
                            time: data[i].time,
                            value: sum / weightSum
                        });
                    }
                } else if (weightType === 'exponential') {
                    // 지수 이동평균 (EMA)
                    const multiplier = 2 / (period + 1);
                    let ema = data[0].close;
                    
                    for (let i = 1; i < data.length; i++) {
                        ema = (data[i].close - ema) * multiplier + ema;
                        if (i >= period - 1) {
                            result.push({
                                time: data[i].time,
                                value: ema
                            });
                        }
                    }
                }
                
                return result;
            };

            // 활성화된 이동평균선만 표시
            Object.entries(maSettings).forEach(([key, setting]) => {
                if (key !== 'weightType' && setting.enabled) {
                    const maSeries = mainChart.current.addLineSeries({
                        color: key === 'ma5' ? '#2962FF' :
                               key === 'ma10' ? '#00C853' :
                               key === 'ma20' ? '#FF9800' :
                               key === 'ma60' ? '#E91E63' :
                               '#9C27B0', // ma120
                        lineWidth: 1,
                        title: `MA${setting.period}`,
                    });

                    maSeries.setData(calculateMA(data, setting.period, maSettings.weightType));
                }
            });
        }

        // MACD 계산 및 추가 (MACD가 선택된 경우에만)
        if (indicators.includes('macd')) {
            const calculateMACD = (data, shortPeriod = 12, longPeriod = 26, signalPeriod = 9) => {
                const ema = (data, period) => {
                    const k = 2 / (period + 1);
                    const result = [{ time: data[0].time, value: data[0].close }];
                    
                    for (let i = 1; i < data.length; i++) {
                        result.push({
                            time: data[i].time,
                            value: data[i].close * k + result[i-1].value * (1-k)
                        });
                    }
                    return result;
                };

                const shortEMA = ema(data, shortPeriod);
                const longEMA = ema(data, longPeriod);
                const macdLine = shortEMA.map((short, i) => ({
                    time: short.time,
                    value: short.value - longEMA[i].value
                }));

                return macdLine;
            };

            // MACD 시리즈 추가
            const macdSeries = mainChart.current.addLineSeries({
                color: '#2196F3',
                lineWidth: 1,
                title: 'MACD',
                priceScaleId: 'macd',
                scaleMargins: {
                    top: 0.7,
                    bottom: 0.2,
                },
            });

            macdSeries.setData(calculateMACD(data));
        }

        // RSI 계산 및 추가 (RSI가 선택된 경우에만)
        if (indicators.includes('rsi')) {
            const calculateRSI = (data, period = 14) => {
                const changes = data.map((d, i) => {
                    if (i === 0) return { gain: 0, loss: 0 };
                    const change = d.close - data[i-1].close;
                    return {
                        gain: change > 0 ? change : 0,
                        loss: change < 0 ? -change : 0
                    };
                });

                const avgGain = changes.slice(1, period + 1).reduce((sum, c) => sum + c.gain, 0) / period;
                const avgLoss = changes.slice(1, period + 1).reduce((sum, c) => sum + c.loss, 0) / period;

                const result = [{
                    time: data[period].time,
                    value: 100 - (100 / (1 + avgGain / avgLoss))
                }];

                for (let i = period + 1; i < data.length; i++) {
                    const change = changes[i];
                    const newAvgGain = (avgGain * (period - 1) + change.gain) / period;
                    const newAvgLoss = (avgLoss * (period - 1) + change.loss) / period;
                    result.push({
                        time: data[i].time,
                        value: 100 - (100 / (1 + newAvgGain / newAvgLoss))
                    });
                }

                return result;
            };

            // RSI 시리즈 추가
            const rsiSeries = mainChart.current.addLineSeries({
                color: '#9C27B0',
                lineWidth: 1,
                title: 'RSI',
                priceScaleId: 'rsi',
                scaleMargins: {
                    top: 0.7,
                    bottom: 0.2,
                },
            });

            rsiSeries.setData(calculateRSI(data));
        }

        const handleResize = () => {
            const width = chartContainerRef.current.clientWidth;
            mainChart.current.applyOptions({ width });
            volumeChart.current.applyOptions({ width });
        };

        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
            mainChart.current.remove();
            volumeChart.current.remove();
        };
    }, [data, indicators, indicatorSettings]);

    return (
        <div className="chart-wrapper" ref={chartContainerRef}>
            <div ref={mainChartRef} style={{ marginBottom: '1px' }} />
            <div ref={volumeChartRef} />
        </div>
    );
};

export default CandleStickChart;
