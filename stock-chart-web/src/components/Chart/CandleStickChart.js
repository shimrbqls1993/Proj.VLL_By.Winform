import React from 'react';
import { createChart } from 'lightweight-charts';
import styled from 'styled-components';
import { 
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
} from './indicators';

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

// 툴팁 스타일 추가
const TooltipContainer = styled.div`
    position: absolute;
    z-index: 200;
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 4px;
    padding: 8px 12px;
    font-size: 12px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
    display: none;
    pointer-events: none;
    min-width: 180px;
`;

const TooltipTitle = styled.div`
    font-weight: bold;
    margin-bottom: 5px;
    border-bottom: 1px solid #eee;
    padding-bottom: 5px;
`;

const TooltipRow = styled.div`
    display: flex;
    justify-content: space-between;
    margin: 2px 0;
`;

const TooltipLabel = styled.span`
    color: #666;
`;

const TooltipValue = styled.span`
    font-weight: 500;
    margin-left: 10px;
`;

const CandleStickChart = ({ data, code, indicators, indicatorSettings }) => {
    const mainChartRef = React.useRef();
    const volumeChartRef = React.useRef();
    const mainChart = React.useRef();
    const volumeChart = React.useRef();
    const chartContainerRef = React.useRef();
    const dragStartPosition = React.useRef(null);
    const selectionBoxRef = React.useRef(null);
    const tooltipRef = React.useRef(null);
    const isDragging = React.useRef(false);
    const lastTooltipData = React.useRef(null);
    const indicatorSeries = React.useRef({});

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

        // 툴팁 요소 생성
        const tooltip = document.createElement('div');
        tooltip.style.position = 'absolute';
        tooltip.style.zIndex = '200';
        tooltip.style.backgroundColor = 'white';
        tooltip.style.border = '1px solid #ddd';
        tooltip.style.borderRadius = '4px';
        tooltip.style.padding = '8px 12px';
        tooltip.style.fontSize = '12px';
        tooltip.style.boxShadow = '0 2px 5px rgba(0, 0, 0, 0.1)';
        tooltip.style.display = 'none';
        tooltip.style.pointerEvents = 'none';
        tooltip.style.minWidth = '180px';
        chartContainerRef.current.appendChild(tooltip);
        tooltipRef.current = tooltip;

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
            indicatorSeries.current.volume = Volume.addToChart(volumeChart.current, data);
        }

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
            if (initialTimeRange && volumeChart.current) {
                try {
                    volumeChart.current.timeScale().setVisibleRange(initialTimeRange);
                } catch (error) {
                    console.log('Initial sync error:', error);
                }
            }
        }, 100);

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
            if (isDragging.current && dragStartPosition.current) {
                const chartRect = chartContainerRef.current.getBoundingClientRect();
                const currentX = e.clientX - chartRect.left;
                const startX = dragStartPosition.current - chartRect.left;

                const left = Math.min(startX, currentX);
                const width = Math.abs(currentX - startX);

                selectionBoxRef.current.style.left = `${left}px`;
                selectionBoxRef.current.style.width = `${width}px`;
                
                // 드래그 중에는 툴팁 숨기기
                tooltipRef.current.style.display = 'none';
            }
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
            // 마우스가 차트를 벗어나면 툴팁 숨기기
            tooltipRef.current.style.display = 'none';
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

        // 캔들 데이터 툴팁 표시 함수
        const updateTooltip = (param) => {
            if (!param.time || isDragging.current) {
                tooltipRef.current.style.display = 'none';
                return;
            }

            const candleData = data.find(d => d.time === param.time);
            if (!candleData) {
                tooltipRef.current.style.display = 'none';
                return;
            }

            lastTooltipData.current = candleData;

            // 날짜 형식 변환 (YYYY.MM.DD 형식)
            let dateStr = param.time;
            if (typeof param.time === 'object' && param.time.year) {
                dateStr = `${param.time.year}.${String(param.time.month).padStart(2, '0')}.${String(param.time.day).padStart(2, '0')}`;
            } else if (typeof param.time === 'number') {
                const date = new Date(param.time * 1000);
                dateStr = `${date.getFullYear()}.${String(date.getMonth() + 1).padStart(2, '0')}.${String(date.getDate()).padStart(2, '0')}`;
            }

            // 거래대금 계산 (거래량 * 종가)
            const tradingValue = (candleData.volume * candleData.close).toLocaleString();
            
            // 툴팁 내용 업데이트
            tooltipRef.current.innerHTML = `
                <div style="font-weight: bold; margin-bottom: 5px; border-bottom: 1px solid #eee; padding-bottom: 5px;">
                    ${dateStr}(${getDayOfWeek(param.time)})
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">시가</span>
                    <span style="font-weight: 500; margin-left: 10px;">${candleData.open.toLocaleString()}</span>
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">고가</span>
                    <span style="font-weight: 500; margin-left: 10px;">${candleData.high.toLocaleString()}</span>
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">저가</span>
                    <span style="font-weight: 500; margin-left: 10px;">${candleData.low.toLocaleString()}</span>
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">종가</span>
                    <span style="font-weight: 500; margin-left: 10px;">${candleData.close.toLocaleString()}</span>
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">거래량</span>
                    <span style="font-weight: 500; margin-left: 10px;">${candleData.volume.toLocaleString()}</span>
                </div>
                <div style="display: flex; justify-content: space-between; margin: 2px 0;">
                    <span style="color: #666;">거래대금</span>
                    <span style="font-weight: 500; margin-left: 10px;">${tradingValue}</span>
                </div>
            `;

            // 툴팁 위치 설정
            const chartRect = chartContainerRef.current.getBoundingClientRect();
            const tooltipRect = tooltipRef.current.getBoundingClientRect();
            
            // 마우스 위치에 따라 툴팁 위치 조정
            let left = param.point.x + 10;
            if (left + tooltipRect.width > chartRect.width) {
                left = param.point.x - tooltipRect.width - 10;
            }
            
            let top = param.point.y - tooltipRect.height - 10;
            if (top < 0) {
                top = param.point.y + 10;
            }
            
            tooltipRef.current.style.left = `${left}px`;
            tooltipRef.current.style.top = `${top}px`;
            tooltipRef.current.style.display = 'block';
        };

        // 요일 구하기 함수
        const getDayOfWeek = (time) => {
            const days = ['일', '월', '화', '수', '목', '금', '토'];
            let date;
            
            if (typeof time === 'object' && time.year) {
                date = new Date(time.year, time.month - 1, time.day);
            } else if (typeof time === 'number') {
                date = new Date(time * 1000);
            } else {
                return '';
            }
            
            return days[date.getDay()];
        };

        // 크로스헤어 이벤트 구독
        mainChart.current.subscribeCrosshairMove(param => {
            updateTooltip(param);
        });

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
            indicatorSeries.current.ma = MA.addToChart(mainChart.current, data, indicatorSettings.ma);
        }

        // 볼린저 밴드 추가 (bollinger가 선택된 경우에만)
        if (indicators.includes('bollinger')) {
            indicatorSeries.current.bollinger = BollingerBands.addToChart(mainChart.current, data, indicatorSettings.bollinger);
        }

        // 파라볼릭 SAR 추가 (parabolicSAR가 선택된 경우에만)
        if (indicators.includes('parabolicSAR')) {
            indicatorSeries.current.parabolicSAR = ParabolicSAR.addToChart(mainChart.current, data, indicatorSettings.parabolicSAR);
        }

        // 엔벨로프 추가 (envelope가 선택된 경우에만)
        if (indicators.includes('envelope')) {
            indicatorSeries.current.envelope = Envelope.addToChart(mainChart.current, data, indicatorSettings.envelope);
        }

        // 피봇 포인트 추가 (pivotPoints가 선택된 경우에만)
        if (indicators.includes('pivotPoints')) {
            indicatorSeries.current.pivotPoints = PivotPoints.addToChart(mainChart.current, data, indicatorSettings.pivotPoints);
        }

        // 가격 채널 추가 (priceChannel이 선택된 경우에만)
        if (indicators.includes('priceChannel')) {
            indicatorSeries.current.priceChannel = PriceChannel.addToChart(mainChart.current, data, indicatorSettings.priceChannel);
        }

        // MACD 계산 및 추가 (MACD가 선택된 경우에만)
        if (indicators.includes('macd')) {
            indicatorSeries.current.macd = MACD.addToChart(mainChart.current, data, indicatorSettings.macd);
        }

        // RSI 계산 및 추가 (RSI가 선택된 경우에만)
        if (indicators.includes('rsi')) {
            indicatorSeries.current.rsi = RSI.addToChart(mainChart.current, data, indicatorSettings.rsi);
        }

        // 스토캐스틱 추가 (stochastic이 선택된 경우에만)
        if (indicators.includes('stochastic')) {
            indicatorSeries.current.stochastic = Stochastic.addToChart(mainChart.current, data, indicatorSettings.stochastic);
        }

        const handleResize = () => {
            const width = chartContainerRef.current.clientWidth;
            mainChart.current.applyOptions({ width });
            volumeChart.current.applyOptions({ width });
        };

        window.addEventListener('resize', handleResize);

        return () => {
            try {
                window.removeEventListener('resize', handleResize);
                document.removeEventListener('mousemove', handleMouseMove);
                document.removeEventListener('mouseup', handleMouseUp);
                
                if (mainChartRef.current) {
                    mainChartRef.current.removeEventListener('mousedown', handleMouseDown);
                    mainChartRef.current.removeEventListener('mouseleave', handleMouseLeave);
                    mainChartRef.current.removeEventListener('wheel', handleWheel);
                }
                
                if (volumeChartRef.current) {
                    volumeChartRef.current.removeEventListener('mousedown', handleMouseDown);
                    volumeChartRef.current.removeEventListener('mouseleave', handleMouseLeave);
                    volumeChartRef.current.removeEventListener('wheel', handleWheel);
                }
                
                // 지표 시리즈 제거 - 안전하게 처리
                try {
                    if (indicatorSeries.current.volume && volumeChart.current) {
                        Volume.removeFromChart(volumeChart.current, indicatorSeries.current.volume);
                    }
                } catch (error) {
                    console.warn('Volume 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.ma && mainChart.current) {
                        MA.removeFromChart(mainChart.current, indicatorSeries.current.ma);
                    }
                } catch (error) {
                    console.warn('MA 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.bollinger && mainChart.current) {
                        BollingerBands.removeFromChart(mainChart.current, indicatorSeries.current.bollinger);
                    }
                } catch (error) {
                    console.warn('볼린저 밴드 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.parabolicSAR && mainChart.current) {
                        ParabolicSAR.removeFromChart(mainChart.current, indicatorSeries.current.parabolicSAR);
                    }
                } catch (error) {
                    console.warn('파라볼릭 SAR 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.envelope && mainChart.current) {
                        Envelope.removeFromChart(mainChart.current, indicatorSeries.current.envelope);
                    }
                } catch (error) {
                    console.warn('엔벨로프 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.pivotPoints && mainChart.current) {
                        PivotPoints.removeFromChart(mainChart.current, indicatorSeries.current.pivotPoints);
                    }
                } catch (error) {
                    console.warn('피봇 포인트 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.priceChannel && mainChart.current) {
                        PriceChannel.removeFromChart(mainChart.current, indicatorSeries.current.priceChannel);
                    }
                } catch (error) {
                    console.warn('가격 채널 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.macd && mainChart.current) {
                        MACD.removeFromChart(mainChart.current, indicatorSeries.current.macd);
                    }
                } catch (error) {
                    console.warn('MACD 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.rsi && mainChart.current) {
                        RSI.removeFromChart(mainChart.current, indicatorSeries.current.rsi);
                    }
                } catch (error) {
                    console.warn('RSI 시리즈 제거 오류:', error);
                }
                
                try {
                    if (indicatorSeries.current.stochastic && mainChart.current) {
                        Stochastic.removeFromChart(mainChart.current, indicatorSeries.current.stochastic);
                    }
                } catch (error) {
                    console.warn('스토캐스틱 시리즈 제거 오류:', error);
                }
                
                // 차트 제거
                if (mainChart.current) {
                    try {
                        mainChart.current.remove();
                    } catch (error) {
                        console.warn('메인 차트 제거 오류:', error);
                    }
                }
                
                if (volumeChart.current) {
                    try {
                        volumeChart.current.remove();
                    } catch (error) {
                        console.warn('거래량 차트 제거 오류:', error);
                    }
                }
                
                // DOM 요소 제거
                if (selectionBoxRef.current) {
                    try {
                        selectionBoxRef.current.remove();
                    } catch (error) {
                        console.warn('선택 영역 요소 제거 오류:', error);
                    }
                }
                
                if (tooltipRef.current) {
                    try {
                        tooltipRef.current.remove();
                    } catch (error) {
                        console.warn('툴팁 요소 제거 오류:', error);
                    }
                }
            } catch (error) {
                console.error('차트 정리 중 오류 발생:', error);
            }
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
