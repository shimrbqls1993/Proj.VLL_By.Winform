import React, { useState, useEffect } from 'react';
import styled from 'styled-components';

const Container = styled.div`
    width: 300px;
    padding: 20px;
    background: white;
    border-left: 1px solid #e0e0e0;
`;

const Section = styled.div`
    margin-bottom: 20px;
`;

const Title = styled.h3`
    font-size: 16px;
    font-weight: bold;
    margin-bottom: 15px;
    color: #333;
`;

const InfoRow = styled.div`
    display: flex;
    justify-content: space-between;
    margin-bottom: 8px;
`;

const Label = styled.span`
    color: #666;
`;

const Value = styled.span`
    font-weight: 500;
    color: ${props => {
        if (props.type === 'change') {
            return props.value > 0 ? '#ff1744' : props.value < 0 ? '#1e88e5' : '#333';
        }
        return '#333';
    }};
`;

const formatNumber = (num) => {
    if (num === undefined || num === null) return '-';
    if (typeof num === 'number') {
        if (num >= 100000000) {
            return (num / 100000000).toFixed(2) + '억';
        }
        return num.toLocaleString();
    }
    return num;
};

const MarketInfo = ({ code }) => {
    const [stockInfo, setStockInfo] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchStockInfo = async () => {
            try {
                if (!code) return;
                
                // API URL 환경변수 사용
                const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:3001/api';
                console.log('Fetching stock info for:', code);
                console.log('API URL:', `${API_URL}/stocks/info/${code}`);
                
                const response = await fetch(`${API_URL}/stocks/info/${code}`);
                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.error || 'Failed to fetch stock info');
                }
                const data = await response.json();
                console.log('Received stock info:', data);
                setStockInfo(data);
                setError(null);
            } catch (error) {
                console.error('Error fetching stock info:', error);
                setError(error.message);
                setStockInfo(null);
            }
        };

        fetchStockInfo();
    }, [code]);

    if (error) return (
        <Container>
            <div style={{ color: 'red' }}>Error: {error}</div>
        </Container>
    );

    if (!stockInfo) return (
        <Container>
            <div>Loading stock information...</div>
        </Container>
    );

    return (
        <Container>
            <Section>
                <Title>종목정보</Title>
                <InfoRow>
                    <Label>종목코드</Label>
                    <Value>{stockInfo.종목코드}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>현재가</Label>
                    <Value>{formatNumber(stockInfo.현재가)}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>등락률</Label>
                    <Value type="change" value={stockInfo.등락률}>
                        {stockInfo.등락률 > 0 ? '+' : ''}{stockInfo.등락률}%
                    </Value>
                </InfoRow>
                <InfoRow>
                    <Label>거래량</Label>
                    <Value>{formatNumber(stockInfo.거래량)}</Value>
                </InfoRow>
            </Section>

            <Section>
                <Title>기업정보</Title>
                <InfoRow>
                    <Label>시가총액</Label>
                    <Value>{formatNumber(stockInfo.시가총액)}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>외국인지분율</Label>
                    <Value>{stockInfo.외국인지분율?.toFixed(2)}%</Value>
                </InfoRow>
                <InfoRow>
                    <Label>52주 최고</Label>
                    <Value>{formatNumber(stockInfo['52주_최고'])}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>52주 최저</Label>
                    <Value>{formatNumber(stockInfo['52주_최저'])}</Value>
                </InfoRow>
            </Section>
        </Container>
    );
};

export default MarketInfo; 