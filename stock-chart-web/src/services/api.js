const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:3001/api';

export const searchStocks = async (query) => {
    try {
        console.log('API URL:', API_URL);
        console.log('Search query:', query);
        const response = await fetch(`${API_URL}/stocks/search?query=${encodeURIComponent(query)}`);
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error('Search API error:', error);
        throw new Error(`Failed to search stocks: ${error.message}`);
    }
};

export const fetchStockData = async (code) => {
    try {
        console.log('Fetching stock data for:', code);
        const response = await fetch(`${API_URL}/stocks/fetch/${code}`);
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error('Fetch API error:', error);
        throw new Error(`Failed to fetch stock data: ${error.message}`);
    }
};

export const getCandleStickData = async (code, timeFrame) => {
    try {
        const response = await fetch(
            `${API_URL}/candlesticks/${code}/${timeFrame}`
        );
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error || 'Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error('Candlestick API error:', error);
        throw new Error(`Failed to fetch candlestick data: ${error.message}`);
    }
};
