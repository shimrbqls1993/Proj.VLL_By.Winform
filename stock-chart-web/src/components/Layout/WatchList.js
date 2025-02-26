import React from 'react';
import styled from 'styled-components';

const WatchListContainer = styled.div`
    width: 300px;
    height: calc(100vh - 60px);
    background-color: #fff;
    border-right: 1px solid #e0e0e0;
    overflow-y: auto;
`;

const WatchListHeader = styled.div`
    padding: 15px;
    border-bottom: 1px solid #e0e0e0;
    font-weight: bold;
`;

const StockItem = styled.div`
    padding: 15px;
    border-bottom: 1px solid #f5f5f5;
    cursor: pointer;
    display: flex;
    justify-content: space-between;
    align-items: center;

    &:hover {
        background-color: #f8f9fa;
    }
`;

const StockName = styled.div`
    font-weight: 500;
`;

const StockPrice = styled.div`
    color: ${props => props.isUp ? '#ff1744' : '#1e88e5'};
`;

const AddButton = styled.button`
    margin: 15px;
    padding: 8px 16px;
    background-color: #2962FF;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    width: calc(100% - 30px);

    &:hover {
        background-color: #1e4bd8;
    }
`;

const WatchList = ({ onSelectStock }) => {
    const [watchList, setWatchList] = React.useState([
        { code: '005930', name: '삼성전자', price: 58200, change: -0.34 },
        { code: '000660', name: 'SK하이닉스', price: 155600, change: 2.1 },
        { code: '035720', name: '카카오', price: 47150, change: -1.2 },
    ]);

    return (
        <WatchListContainer>
            <WatchListHeader>관심종목</WatchListHeader>
            {watchList.map(stock => (
                <StockItem key={stock.code} onClick={() => onSelectStock(stock.code)}>
                    <StockName>{stock.name}</StockName>
                    <StockPrice isUp={stock.change > 0}>
                        {stock.price.toLocaleString()}
                        <span style={{ marginLeft: '5px', fontSize: '0.9em' }}>
                            {stock.change > 0 ? `+${stock.change}` : stock.change}%
                        </span>
                    </StockPrice>
                </StockItem>
            ))}
            <AddButton>종목 추가</AddButton>
        </WatchListContainer>
    );
};

export default WatchList; 