import React from 'react';
import { createChart } from 'lightweight-charts';

const CandleStickChart = ({ data, code }) => {
    const chartContainerRef = React.useRef();
    const chart = React.useRef();

    React.useEffect(() => {
        if (!data || data.length === 0) return;

        chart.current = createChart(chartContainerRef.current, {
            width: chartContainerRef.current.clientWidth,
            height: 600,
            layout: {
                background: { color: '#ffffff' },
                textColor: '#333',
            },
            grid: {
                vertLines: { color: '#f0f0f0' },
                horzLines: { color: '#f0f0f0' },
            },
            crosshair: {
                mode: 1,
                vertLine: {
                    width: 1,
                    color: '#2962FF',
                    style: 0,
                },
                horzLine: {
                    width: 1,
                    color: '#2962FF',
                    style: 0,
                },
            },
            timeScale: {
                timeVisible: true,
                secondsVisible: false,
            },
        });

        const candleSeries = chart.current.addCandlestickSeries({
            upColor: '#26a69a',
            downColor: '#ef5350',
            borderVisible: false,
            wickUpColor: '#26a69a',
            wickDownColor: '#ef5350'
        });

        const volumeSeries = chart.current.addHistogramSeries({
            color: '#26a69a',
            priceFormat: {
                type: 'volume',
            },
            priceScaleId: 'volume',
            scaleMargins: {
                top: 0.8,
                bottom: 0,
            },
        });

        candleSeries.setData(data);
        volumeSeries.setData(
            data.map(item => ({
                time: item.time,
                value: item.volume,
                color: item.close >= item.open ? '#26a69a' : '#ef5350'
            }))
        );

        const handleResize = () => {
            chart.current.applyOptions({
                width: chartContainerRef.current.clientWidth
            });
        };

        window.addEventListener('resize', handleResize);

        return () => {
            window.removeEventListener('resize', handleResize);
            chart.current.remove();
        };
    }, [data]);

    return <div className="chart-wrapper" ref={chartContainerRef} />;
};

export default CandleStickChart;
