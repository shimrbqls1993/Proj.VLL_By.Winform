import React from 'react';

const TimeFrameSelector = ({ selectedTimeFrame, onTimeFrameChange }) => {
    const timeFrames = [
        { value: 'MIN_1', label: '1분' },
        { value: 'MIN_5', label: '5분' },
        { value: 'HOUR', label: '1시간' },
        { value: 'DAY', label: '일' },
        { value: 'WEEK', label: '주' },
    ];

    return (
        <div className="timeframe-selector">
            {timeFrames.map(tf => (
                <button
                    key={tf.value}
                    className={`timeframe-button ${selectedTimeFrame === tf.value ? 'active' : ''}`}
                    onClick={() => onTimeFrameChange(tf.value)}
                >
                    {tf.label}
                </button>
            ))}
        </div>
    );
};

export default TimeFrameSelector;
