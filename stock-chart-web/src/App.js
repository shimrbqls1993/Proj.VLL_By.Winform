import React, { useState } from 'react';
import ChartContainer from './components/Chart/ChartContainer';
import './styles/Chart.css';

function App() {
    const [code, setCode] = useState('005930');

    const handleCodeChange = (e) => {
        setCode(e.target.value);
    };

    return (
        <div className="app-container">
            <h1 className="title">주식 차트</h1>
            <div className="code-input-container">
                <input 
                    type="text" 
                    value={code} 
                    onChange={handleCodeChange}
                    placeholder="종목 코드 입력"
                    className="code-input"
                />
            </div>
            <ChartContainer code={code} />
        </div>
    );
}

export default App;
