import React from 'react';
import { createChart } from 'lightweight-charts';
import styled from 'styled-components';

const ChartWrapper = styled.div`
    position: relative;
    width: 100%;
    height: 600px;
    user-select: none;
`;

const SelectionBox = styled.div`
    position: absolute;
    background-color: rgba(41, 98, 255, 0.1);
    border: 1px solid rgba(41, 98, 255, 0.3);
    pointer-events: none;
    display: none;
    z-index: 100;
`;

const CandleStickChart = ({ data, code, indicators, indicatorSettings }) => {
    const mainChartRef = React.useRef();
    const volumeChartRef = React.useRef();
    const mainChart = React.useRef();
    const volumeChart = React.useRef();
    const chartContainerRef = React.useRef();
    const dragStartPosition = React.useRef(null);
    const selectionBoxRef = React.useRef(null);
    const isDragging = React.useRef(false);

    React.useEffect(() => {
        if (!data || data.length === 0) return;

        // 선택 영역 요소 생성
        const selectionBox = document.createElement('div');
        selectionBox.style.position = 'absolute';
        selectionBox.style.backgroundColor = 'rgba(41, 98, 255, 0.1)';
        selectionBox.style.border = '1px solid rgba(41, 98, 255, 0.3)';
        selectionBox.style.pointerEvents = 'none';
        selectionBox.style.display = 'none';
        selectionBox.style.zIndex = '100';
        chartContainerRef.current.appendChild(selectionBox);
        selectionBoxRef.current = selectionBox;

        // 메인 차트 생성 (가격)
        mainChart.current = createChart(mainChartRef.current, {
            width: chartContainerRef.current.clientWidth,
            height: 480,
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
            handleScroll: {
                mouseWheel: false,
                pressedMouseMove: false,
            },
            handleScale: {
                mouseWheel: false,
                pinch: true,
                axisPressedMouseMove: false,
            },
        });

        // 거래량 차트 생성
        volumeChart.current = createChart(volumeChartRef.current, {
            width: chartContainerRef.current.clientWidth,
            height: 120,
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
                visible: false,
            },
            rightPriceScale: {
                borderColor: '#D1D4DC',
                borderVisible: true,
                scaleMargins: {
                    top: 0.1,
                    bottom: 0.1,
                },
            },
            handleScroll: {
                mouseWheel: false,
                pressedMouseMove: false,
            },
            handleScale: {
                mouseWheel: false,
                pinch: true,
                axisPressedMouseMove: false,
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

        // 마우스 이벤트 핸들러 수정
        const handleMouseDown = (e) => {
            e.preventDefault();
            dragStartPosition.current = e.clientX;
            isDragging.current = true;
            
            const chartRect = chartContainerRef.current.getBoundingClientRect();
            const startX = e.clientX - chartRect.left;
            
            selectionBoxRef.current.style.left = `${startX}px`;
            selectionBoxRef.current.style.top = '0';
            selectionBoxRef.current.style.width = '0';
            selectionBoxRef.current.style.height = '100%';
            selectionBoxRef.current.style.display = 'block';
        };

        const handleMouseMove = (e) => {
            if (!isDragging.current || !dragStartPosition.current) return;

            const chartRect = chartContainerRef.current.getBoundingClientRect();
            const currentX = e.clientX - chartRect.left;
            const startX = dragStartPosition.current - chartRect.left;

            const left = Math.min(startX, currentX);
            const width = Math.abs(currentX - startX);

            selectionBoxRef.current.style.left = `${left}px`;
            selectionBoxRef.current.style.width = `${width}px`;
        };

        const handleMouseUp = (e) => {
            if (!isDragging.current || !dragStartPosition.current) return;

            const dragEndPosition = e.clientX;
            const dragDistance = dragEndPosition - dragStartPosition.current;
            const timeScale = mainChart.current.timeScale();
            const currentRange = timeScale.getVisibleLogicalRange();
            
            if (Math.abs(dragDistance) > 5) {
                // 현재 표시되는 캔들 개수 계산
                const currentBars = Math.ceil(currentRange.to - currentRange.from);
                
                // 드래그 거리에 따른 변화 비율 계산 (30% ~ 50%)
                const zoomRatio = Math.min(0.5, Math.max(0.3, Math.abs(dragDistance) / 300));
                const changeAmount = Math.max(1, Math.floor(currentBars * zoomRatio));
                
                let newBars;
                if (dragDistance < 0) {  // 오른쪽에서 왼쪽으로 드래그 = 캔들 개수 증가
                    newBars = currentBars + changeAmount;
                } else {  // 왼쪽에서 오른쪽으로 드래그 = 캔들 개수 감소
                    newBars = currentBars - changeAmount;
                    // 최소 30개 캔들 제한
                    if (newBars < 30) {
                        newBars = 30;
                    }
                }
                
                // 중심점 계산
                const center = Math.floor((currentRange.from + currentRange.to) / 2);
                
                // 새로운 범위 계산
                const halfBars = Math.floor(newBars / 2);
                const newRange = {
                    from: Math.max(0, center - halfBars),
                    to: center + halfBars
                };
                
                // 범위가 유효한지 최종 확인
                if (newRange.to > newRange.from) {
                    try {
                        timeScale.setVisibleLogicalRange(newRange);
                    } catch (error) {
                        console.warn('Zoom operation failed:', error);
                    }
                }
            }
            
            selectionBoxRef.current.style.display = 'none';
            dragStartPosition.current = null;
            isDragging.current = false;
        };

        const handleMouseLeave = () => {
            if (isDragging.current) {
                selectionBoxRef.current.style.display = 'none';
                dragStartPosition.current = null;
                isDragging.current = false;
            }
        };

        const handleWheel = (e) => {
            e.preventDefault();
            const timeScale = mainChart.current.timeScale();
            const logicalRange = timeScale.getVisibleLogicalRange();
            const dataLength = data.length;
            
            // 현재 보이는 캔들 개수 계산
            const visibleBars = logicalRange.to - logicalRange.from;
            // 이동할 스텝 크기 (보이는 캔들의 10%)
            const scrollStep = Math.max(1, Math.floor(visibleBars * 0.1));
            
            if (e.deltaY < 0) { // 스크롤 업 (과거)
                const newRange = {
                    from: logicalRange.from - scrollStep,
                    to: logicalRange.to - scrollStep
                };
                // 왼쪽 경계 체크
                if (newRange.from >= 0) {
                    try {
                        timeScale.setVisibleLogicalRange(newRange);
                    } catch (error) {
                        console.warn('Scroll operation failed:', error);
                    }
                }
            } else { // 스크롤 다운 (최신)
                const newRange = {
                    from: logicalRange.from + scrollStep,
                    to: logicalRange.to + scrollStep
                };
                // 오른쪽 경계 체크
                if (newRange.to <= dataLength) {
                    try {
                        timeScale.setVisibleLogicalRange(newRange);
                    } catch (error) {
                        console.warn('Scroll operation failed:', error);
                    }
                }
            }
        };

        // 이벤트 리스너 추가
        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);
        mainChartRef.current.addEventListener('mousedown', handleMouseDown);
        mainChartRef.current.addEventListener('mouseleave', handleMouseLeave);
        mainChartRef.current.addEventListener('wheel', handleWheel);
        volumeChartRef.current.addEventListener('mousedown', handleMouseDown);
        volumeChartRef.current.addEventListener('mouseleave', handleMouseLeave);
        volumeChartRef.current.addEventListener('wheel', handleWheel);

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
            document.removeEventListener('mousemove', handleMouseMove);
            document.removeEventListener('mouseup', handleMouseUp);
            mainChartRef.current?.removeEventListener('mousedown', handleMouseDown);
            mainChartRef.current?.removeEventListener('mouseleave', handleMouseLeave);
            mainChartRef.current?.removeEventListener('wheel', handleWheel);
            volumeChartRef.current?.removeEventListener('mousedown', handleMouseDown);
            volumeChartRef.current?.removeEventListener('mouseleave', handleMouseLeave);
            volumeChartRef.current?.removeEventListener('wheel', handleWheel);
            mainChart.current?.remove();
            volumeChart.current?.remove();
            
            // 선택 영역 요소 제거
            selectionBoxRef.current?.remove();
        };
    }, [data, indicators, indicatorSettings]);

    return (
        <ChartWrapper ref={chartContainerRef}>
            <div ref={mainChartRef} style={{ marginBottom: '1px' }} />
            <div ref={volumeChartRef} />
        </ChartWrapper>
    );
};

export default CandleStickChart;
