import React, { useState, useEffect, useRef } from 'react';

const TimeFrameSelector = ({ selectedTimeFrame, onTimeFrameChange }) => {
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const dropdownRef = useRef(null);
    const [availableTimeFrames, setAvailableTimeFrames] = useState([
        { value: 'MIN_1', label: '1분', displayLabel: '1분', favorite: true },
        { value: 'MIN_3', label: '3분', displayLabel: '3분', favorite: false },
        { value: 'MIN_5', label: '5분', displayLabel: '5분', favorite: false },
        { value: 'MIN_10', label: '10분', displayLabel: '10분', favorite: false },
        { value: 'MIN_15', label: '15분', displayLabel: '15분', favorite: true },
        { value: 'MIN_30', label: '30분', displayLabel: '30분', favorite: false },
        { value: 'HOUR', label: '1시간(1시)', displayLabel: '1시', favorite: false },
        { value: 'HOUR_2', label: '2시간(2시)', displayLabel: '2시', favorite: false },
        { value: 'HOUR_4', label: '4시간(4시)', displayLabel: '4시', favorite: false },
        { value: 'DAY', label: '1일(일)', displayLabel: '일', favorite: true },
        { value: 'WEEK', label: '7일(주)', displayLabel: '주', favorite: true },
        { value: 'MONTH', label: '30일(월)', displayLabel: '월', favorite: true },
        { value: 'YEAR', label: '365일(년)', displayLabel: '년', favorite: false },
    ]);

    // 외부 클릭 시 드롭다운 닫기
    useEffect(() => {
        const handleClickOutside = (event) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
                setIsDropdownOpen(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    // 즐겨찾기된 시간 프레임만 표시
    const favoriteTimeFrames = availableTimeFrames.filter(tf => tf.favorite);

    const toggleFavorite = (value) => {
        setAvailableTimeFrames(prev => 
            prev.map(tf => 
                tf.value === value 
                    ? { ...tf, favorite: !tf.favorite } 
                    : tf
            )
        );
    };

    return (
        <div style={{ position: 'relative' }} ref={dropdownRef}>
            <div className="timeframe-selector" style={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center' }}>
                {favoriteTimeFrames.map(tf => (
                    <button
                        key={tf.value}
                        className={`timeframe-button ${selectedTimeFrame === tf.value ? 'active' : ''}`}
                        onClick={() => onTimeFrameChange(tf.value)}
                    >
                        {tf.displayLabel}
                    </button>
                ))}
                <button 
                    className="timeframe-button dropdown-button"
                    onClick={() => setIsDropdownOpen(prev => !prev)}
                >
                    <svg 
                        xmlns="http://www.w3.org/2000/svg" 
                        width="16" 
                        height="16" 
                        viewBox="0 0 24 24" 
                        fill="none" 
                        stroke="currentColor" 
                        strokeWidth="2" 
                        strokeLinecap="round" 
                        strokeLinejoin="round"
                    >
                        <polyline points="6 9 12 15 18 9"></polyline>
                    </svg>
                </button>
            </div>

            {isDropdownOpen && (
                <div className="timeframe-dropdown" style={{
                    position: 'absolute',
                    right: 0,
                    top: '100%',
                    width: '200px',
                    backgroundColor: 'white',
                    boxShadow: '0 2px 10px rgba(0,0,0,0.1)',
                    borderRadius: '4px',
                    zIndex: 1000,
                    marginTop: '5px',
                    padding: '10px 0',
                }}>
                    <div style={{ padding: '5px 15px', borderBottom: '1px solid #eee', fontWeight: 'bold', color: '#888' }}>
                        분봉
                    </div>
                    {availableTimeFrames.filter(tf => tf.value.startsWith('MIN')).map(tf => (
                        <div key={tf.value} style={{ 
                            display: 'flex', 
                            justifyContent: 'space-between', 
                            alignItems: 'center',
                            padding: '8px 15px',
                            cursor: 'pointer',
                            backgroundColor: selectedTimeFrame === tf.value ? '#f5f5f5' : 'transparent',
                        }}
                        onClick={() => {
                            onTimeFrameChange(tf.value);
                            setIsDropdownOpen(false);
                        }}
                        >
                            <span>{tf.label}</span>
                            <span 
                                onClick={(e) => {
                                    e.stopPropagation();
                                    toggleFavorite(tf.value);
                                }}
                                style={{ color: tf.favorite ? 'gold' : '#ccc', cursor: 'pointer' }}
                            >
                                ★
                            </span>
                        </div>
                    ))}

                    <div style={{ padding: '5px 15px', borderBottom: '1px solid #eee', fontWeight: 'bold', color: '#888', marginTop: '10px' }}>
                        시간봉
                    </div>
                    {availableTimeFrames.filter(tf => tf.value.startsWith('HOUR')).map(tf => (
                        <div key={tf.value} style={{ 
                            display: 'flex', 
                            justifyContent: 'space-between', 
                            alignItems: 'center',
                            padding: '8px 15px',
                            cursor: 'pointer',
                            backgroundColor: selectedTimeFrame === tf.value ? '#f5f5f5' : 'transparent',
                        }}
                        onClick={() => {
                            onTimeFrameChange(tf.value);
                            setIsDropdownOpen(false);
                        }}
                        >
                            <span>{tf.label}</span>
                            <span 
                                onClick={(e) => {
                                    e.stopPropagation();
                                    toggleFavorite(tf.value);
                                }}
                                style={{ color: tf.favorite ? 'gold' : '#ccc', cursor: 'pointer' }}
                            >
                                ★
                            </span>
                        </div>
                    ))}

                    <div style={{ padding: '5px 15px', borderBottom: '1px solid #eee', fontWeight: 'bold', color: '#888', marginTop: '10px' }}>
                        일봉
                    </div>
                    {availableTimeFrames.filter(tf => ['DAY', 'WEEK', 'MONTH', 'YEAR'].includes(tf.value)).map(tf => (
                        <div key={tf.value} style={{ 
                            display: 'flex', 
                            justifyContent: 'space-between', 
                            alignItems: 'center',
                            padding: '8px 15px',
                            cursor: 'pointer',
                            backgroundColor: selectedTimeFrame === tf.value ? '#f5f5f5' : 'transparent',
                        }}
                        onClick={() => {
                            onTimeFrameChange(tf.value);
                            setIsDropdownOpen(false);
                        }}
                        >
                            <span>{tf.label}</span>
                            <span 
                                onClick={(e) => {
                                    e.stopPropagation();
                                    toggleFavorite(tf.value);
                                }}
                                style={{ color: tf.favorite ? 'gold' : '#ccc', cursor: 'pointer' }}
                            >
                                ★
                            </span>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default TimeFrameSelector;
