import React from 'react';
import styled from 'styled-components';

const MarketInfoContainer = styled.div`
    width: 300px;
    height: calc(100vh - 60px);
    background-color: #fff;
    border-left: 1px solid #e0e0e0;
    overflow-y: auto;
`;

const InfoSection = styled.div`
    padding: 20px;
    border-bottom: 1px solid #e0e0e0;
`;

const SectionTitle = styled.h3`
    margin: 0 0 15px 0;
    font-size: 16px;
    color: #333;
`;

const InfoRow = styled.div`
    display: flex;
    justify-content: space-between;
    margin-bottom: 10px;
    font-size: 14px;
`;

const Label = styled.span`
    color: #666;
`;

const Value = styled.span`
    color: #333;
    font-weight: ${props => props.bold ? '500' : 'normal'};
    color: ${props => {
        if (props.isUp) return '#ff1744';
        if (props.isDown) return '#1e88e5';
        return '#333';
    }};
`;

const MarketInfo = ({ stockInfo }) => {
    const defaultInfo = {
        code: '005930',
        name: '삼성전자',
        price: 58200,
        change: -0.34,
        volume: 19281460,
        marketCap: '347조 4,413억',
        foreignOwnership: '50.04%',
        high52: 88800,
        low52: 49900,
    };

    const info = stockInfo || defaultInfo;

    return (
        <MarketInfoContainer>
            <InfoSection>
                <SectionTitle>종목정보</SectionTitle>
                <InfoRow>
                    <Label>종목코드</Label>
                    <Value>{info.code}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>현재가</Label>
                    <Value isDown={info.change < 0} isUp={info.change > 0} bold>
                        {info.price.toLocaleString()}
                    </Value>
                </InfoRow>
                <InfoRow>
                    <Label>등락률</Label>
                    <Value isDown={info.change < 0} isUp={info.change > 0}>
                        {info.change > 0 ? `+${info.change}` : info.change}%
                    </Value>
                </InfoRow>
                <InfoRow>
                    <Label>거래량</Label>
                    <Value>{info.volume.toLocaleString()}</Value>
                </InfoRow>
            </InfoSection>

            <InfoSection>
                <SectionTitle>기업정보</SectionTitle>
                <InfoRow>
                    <Label>시가총액</Label>
                    <Value>{info.marketCap}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>외국인지분율</Label>
                    <Value>{info.foreignOwnership}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>52주 최고</Label>
                    <Value>{info.high52.toLocaleString()}</Value>
                </InfoRow>
                <InfoRow>
                    <Label>52주 최저</Label>
                    <Value>{info.low52.toLocaleString()}</Value>
                </InfoRow>
            </InfoSection>
        </MarketInfoContainer>
    );
};

export default MarketInfo; 