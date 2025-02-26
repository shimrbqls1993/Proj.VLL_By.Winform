const getCandleStickData = async (code, timeFrame) => {
    try {
        const response = await fetch(
            `${process.env.REACT_APP_API_URL}/candlesticks/${code}/${timeFrame}`
        );
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        throw new Error(`Failed to fetch candlestick data: ${error.message}`);
    }
};

export { getCandleStickData };
