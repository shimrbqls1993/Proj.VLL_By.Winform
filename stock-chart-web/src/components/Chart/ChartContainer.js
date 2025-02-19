import React from 'react';
import CandleStickChart from './CandleStickChart';
import TimeFrameSelector from './TimeFrameSelector';
import Loading from '../Common/Loading';

const ChartContainer = ({ code }) => {
    const [timeFrame, setTimeFrame] = React.useState('DAY');
    const [chartData, setChartData] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const [error, setError] = React.useState(null);

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
            <TimeFrameSelector
                selectedTimeFrame={timeFrame}
                onTimeFrameChange={setTimeFrame}
            />
            <CandleStickChart data={chartData} code={code} />
        </div>
    );
};

export default ChartContainer;
