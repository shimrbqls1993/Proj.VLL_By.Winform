import React, { useState } from 'react';
import styled from 'styled-components';
import Header from './components/Layout/Header';
import WatchList from './components/Layout/WatchList';
import ChartContainer from './components/Chart/ChartContainer';
import MarketInfo from './components/Layout/MarketInfo';
import './styles/Chart.css';

const AppContainer = styled.div`
    display: flex;
    flex-direction: column;
    height: 100vh;
`;

const MainContent = styled.div`
    display: flex;
    flex: 1;
    overflow: hidden;
`;

const ChartSection = styled.div`
    flex: 1;
    overflow: hidden;
`;

function App() {
    const [code, setCode] = useState('005930');

    const handleStockSelect = (newCode) => {
        setCode(newCode);
    };

    return (
        <AppContainer>
            <Header onSelectStock={handleStockSelect} />
            <MainContent>
                <WatchList onSelectStock={handleStockSelect} />
                <ChartSection>
                    <ChartContainer code={code} />
                </ChartSection>
                <MarketInfo code={code} />
            </MainContent>
        </AppContainer>
    );
}

export default App;
