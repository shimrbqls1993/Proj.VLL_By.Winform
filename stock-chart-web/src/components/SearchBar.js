import React, { useState, useEffect, useRef } from 'react';
import styled from 'styled-components';
import { searchStocks, fetchStockData } from '../services/api';

const SearchContainer = styled.div`
    position: relative;
    width: 300px;
`;

const Input = styled.input`
    width: 100%;
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
    &:focus {
        outline: none;
        border-color: #666;
    }
`;

const SearchResults = styled.div`
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: white;
    border: 1px solid #ddd;
    border-radius: 4px;
    margin-top: 4px;
    max-height: 300px;
    overflow-y: auto;
    z-index: 1000;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
`;

const SearchItem = styled.div`
    padding: 8px 12px;
    cursor: pointer;
    &:hover {
        background: #f5f5f5;
    }
    display: flex;
    justify-content: space-between;
`;

const StockName = styled.span`
    font-weight: bold;
`;

const StockCode = styled.span`
    color: #666;
    margin-left: 8px;
`;

const SearchBar = ({ onSelectStock }) => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [showResults, setShowResults] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [selectedStock, setSelectedStock] = useState(null);
    const searchContainerRef = useRef(null);

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (searchContainerRef.current && !searchContainerRef.current.contains(event.target)) {
                setShowResults(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    useEffect(() => {
        const search = async () => {
            if (query.length >= 2) {
                setIsLoading(true);
                setError(null);
                try {
                    console.log('Searching for:', query);
                    const data = await searchStocks(query);
                    console.log('Search results:', data);
                    setResults(data.stocks);
                    setShowResults(true);
                } catch (error) {
                    console.error('Search failed:', error);
                    setError(error.message);
                    setResults([]);
                } finally {
                    setIsLoading(false);
                }
            } else {
                setResults([]);
                setShowResults(false);
            }
        };

        const timeoutId = setTimeout(search, 300);
        return () => clearTimeout(timeoutId);
    }, [query]);

    const handleStockSelect = async (stock) => {
        try {
            setSelectedStock(stock);
            setQuery(`${stock.name} (${stock.code})`);
            setShowResults(false);
            console.log('Fetching data for:', stock.code);
            await fetchStockData(stock.code);
            onSelectStock(stock.code);
        } catch (error) {
            console.error('Failed to fetch stock data:', error);
            setError(error.message);
        }
    };

    const handleKeyDown = async (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            if (results.length > 0) {
                // 첫 번째 검색 결과 선택
                const firstStock = results[0];
                await handleStockSelect(firstStock);
            }
        }
    };

    const handleInputChange = (e) => {
        const value = e.target.value;
        setQuery(value);
        // 입력값이 변경되면 선택된 종목 초기화
        setSelectedStock(null);
    };

    return (
        <SearchContainer ref={searchContainerRef}>
            <Input
                type="text"
                value={query}
                onChange={handleInputChange}
                onKeyDown={handleKeyDown}
                placeholder="종목 검색 (2글자 이상)"
            />
            {isLoading && <div>검색 중...</div>}
            {error && <div style={{ color: 'red' }}>{error}</div>}
            {showResults && results.length > 0 && !selectedStock && (
                <SearchResults>
                    {results.map((stock) => (
                        <SearchItem key={stock.code} onClick={() => handleStockSelect(stock)}>
                            <StockName>{stock.name}</StockName>
                            <StockCode>{stock.code}</StockCode>
                        </SearchItem>
                    ))}
                </SearchResults>
            )}
        </SearchContainer>
    );
};

export default SearchBar; 