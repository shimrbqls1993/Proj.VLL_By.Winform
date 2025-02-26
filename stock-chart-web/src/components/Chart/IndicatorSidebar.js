import React, { useState } from 'react';

const IndicatorSidebar = ({ isOpen, onClose, onSelectIndicator, selectedIndicators, onUpdateSettings }) => {
    const [showSettings, setShowSettings] = useState(false);
    const [selectedIndicator, setSelectedIndicator] = useState(null);
    const [maSettings, setMaSettings] = useState({
        weightType: 'simple', // 'simple', 'weighted', 'exponential'
        ma5: { period: 5, enabled: true },
        ma10: { period: 10, enabled: true },
        ma20: { period: 20, enabled: true },
        ma60: { period: 60, enabled: true },
        ma120: { period: 120, enabled: false }
    });

    const indicators = [
        { 
            id: 'ma', 
            name: '이동평균선', 
            category: '차트지표',
            hasSettings: true,
            settings: maSettings
        },
        { id: 'volume', name: '거래량', category: '보조지표' },
        { id: 'macd', name: 'MACD', category: '보조지표' },
        { id: 'rsi', name: 'RSI', category: '보조지표' },
        { id: 'stochastic', name: 'Stochastic', category: '보조지표' },
        { id: 'bollinger', name: '볼린저밴드', category: '차트지표' },
        { id: 'parabolicSAR', name: 'Parabolic SAR', category: '차트지표' },
        { id: 'envelope', name: 'Envelope', category: '차트지표' },
        { id: 'pivotPoints', name: 'Pivot Points', category: '차트지표' },
        { id: 'priceChannel', name: 'Price Channel', category: '차트지표' }
    ];

    const categories = {
        '차트지표': indicators.filter(i => i.category === '차트지표'),
        '보조지표': indicators.filter(i => i.category === '보조지표'),
    };

    const handleIndicatorClick = (indicator) => {
        onSelectIndicator(indicator.id);
    };

    const handleSettingsClick = (e, indicator) => {
        e.preventDefault();
        e.stopPropagation();
        setSelectedIndicator(indicator);
        setShowSettings(true);
    };

    const handleSettingsSave = () => {
        onUpdateSettings(selectedIndicator.id, maSettings);
        setShowSettings(false);
    };

    const handleMaSettingChange = (maKey, field, value) => {
        setMaSettings(prev => ({
            ...prev,
            [maKey]: {
                ...prev[maKey],
                [field]: field === 'period' ? parseInt(value) : value
            }
        }));
    };

    const handleWeightTypeChange = (type) => {
        setMaSettings(prev => ({
            ...prev,
            weightType: type
        }));
    };

    return (
        <div className={`indicator-sidebar ${isOpen ? 'open' : ''}`}>
            <div className="indicator-header">
                <h3>보조지표</h3>
                <button className="close-button" onClick={onClose}>×</button>
            </div>
            <div className="indicator-search">
                <input type="text" placeholder="지표명 검색" />
            </div>
            <div className="indicator-list">
                {Object.entries(categories).map(([category, items]) => (
                    <div key={category} className="indicator-category">
                        <h4>{category}</h4>
                        {items.map(indicator => (
                            <div key={indicator.id} className="indicator-item">
                                <div className="indicator-row">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={selectedIndicators.includes(indicator.id)}
                                            onChange={() => handleIndicatorClick(indicator)}
                                        />
                                        {indicator.name}
                                    </label>
                                    {indicator.hasSettings && selectedIndicators.includes(indicator.id) && (
                                        <button 
                                            className="settings-button"
                                            onClick={(e) => handleSettingsClick(e, indicator)}
                                        >
                                            ⚙️
                                        </button>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                ))}
            </div>

            {showSettings && selectedIndicator && (
                <div className="settings-popup">
                    <div className="settings-popup-content">
                        <div className="settings-header">
                            <h4>이동평균선 설정</h4>
                            <button onClick={() => setShowSettings(false)}>×</button>
                        </div>
                        <div className="settings-body">
                            <div className="weight-type-selector">
                                <h5>가중치 선택</h5>
                                <div className="weight-type-buttons">
                                    <button
                                        className={maSettings.weightType === 'simple' ? 'active' : ''}
                                        onClick={() => handleWeightTypeChange('simple')}
                                    >
                                        단순
                                    </button>
                                    <button
                                        className={maSettings.weightType === 'weighted' ? 'active' : ''}
                                        onClick={() => handleWeightTypeChange('weighted')}
                                    >
                                        가중
                                    </button>
                                    <button
                                        className={maSettings.weightType === 'exponential' ? 'active' : ''}
                                        onClick={() => handleWeightTypeChange('exponential')}
                                    >
                                        지수
                                    </button>
                                </div>
                            </div>
                            <div className="ma-settings">
                                <div className="ma-setting-group">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={maSettings.ma5.enabled}
                                            onChange={(e) => handleMaSettingChange('ma5', 'enabled', e.target.checked)}
                                        />
                                        <input
                                            type="number"
                                            value={maSettings.ma5.period}
                                            onChange={(e) => handleMaSettingChange('ma5', 'period', e.target.value)}
                                            min="1"
                                        />
                                        일선
                                    </label>
                                </div>
                                <div className="ma-setting-group">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={maSettings.ma10.enabled}
                                            onChange={(e) => handleMaSettingChange('ma10', 'enabled', e.target.checked)}
                                        />
                                        <input
                                            type="number"
                                            value={maSettings.ma10.period}
                                            onChange={(e) => handleMaSettingChange('ma10', 'period', e.target.value)}
                                            min="1"
                                        />
                                        일선
                                    </label>
                                </div>
                                <div className="ma-setting-group">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={maSettings.ma20.enabled}
                                            onChange={(e) => handleMaSettingChange('ma20', 'enabled', e.target.checked)}
                                        />
                                        <input
                                            type="number"
                                            value={maSettings.ma20.period}
                                            onChange={(e) => handleMaSettingChange('ma20', 'period', e.target.value)}
                                            min="1"
                                        />
                                        일선
                                    </label>
                                </div>
                                <div className="ma-setting-group">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={maSettings.ma60.enabled}
                                            onChange={(e) => handleMaSettingChange('ma60', 'enabled', e.target.checked)}
                                        />
                                        <input
                                            type="number"
                                            value={maSettings.ma60.period}
                                            onChange={(e) => handleMaSettingChange('ma60', 'period', e.target.value)}
                                            min="1"
                                        />
                                        일선
                                    </label>
                                </div>
                                <div className="ma-setting-group">
                                    <label>
                                        <input
                                            type="checkbox"
                                            checked={maSettings.ma120.enabled}
                                            onChange={(e) => handleMaSettingChange('ma120', 'enabled', e.target.checked)}
                                        />
                                        <input
                                            type="number"
                                            value={maSettings.ma120.period}
                                            onChange={(e) => handleMaSettingChange('ma120', 'period', e.target.value)}
                                            min="1"
                                        />
                                        일선
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div className="settings-footer">
                            <button onClick={handleSettingsSave}>확인</button>
                            <button onClick={() => setShowSettings(false)}>취소</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default IndicatorSidebar; 