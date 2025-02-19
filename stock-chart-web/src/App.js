import React from 'react';
import ChartContainer from './components/Chart/ChartContainer';
import './styles/Chart.css';

function App() {
    return (
        <div className="app-container">
            <h1 className="title">주식 차트</h1>
            <ChartContainer code="005930" />
        </div>
    );
}

export default App;
