import React, { useState } from 'react';
import styled from 'styled-components';
import { indicatorsList, categorizedIndicators } from './indicators';

// 스타일 컴포넌트 추가
const Sidebar = styled.div`
  position: fixed;
  top: 0;
  left: -300px;
  width: 300px;
  height: 100%;
  background-color: white;
  box-shadow: 2px 0 5px rgba(0, 0, 0, 0.1);
  transition: left 0.3s ease;
  z-index: 1000;
  display: flex;
  flex-direction: column;
  border-right: 1px solid #e0e0e0;
  
  &.open {
    left: 0;
  }
`;

const SidebarHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  border-bottom: 1px solid #eee;
`;

const SearchContainer = styled.div`
  padding: 5px;
  border-bottom: 1px solid #eee;
`;

const SidebarTitle = styled.h3`
  margin: 0;
  font-size: 18px;
`;

const CloseButton = styled.button`
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
  color: #666;
  
  &:hover {
    color: #333;
  }
`;

const SearchInput = styled.input`
  width: 90%;
  margin: 0 auto;
  padding: 8px 10px;
  border: 1px solid #eee;
  border-radius: 4px;
  font-size: 13px;
  display: block;
  margin-top: 10px;
  margin-bottom: 10px;
  
  &:focus {
    outline: none;
    border-color: #2962FF;
  }
`;

const IndicatorList = styled.div`
  flex: 1;
  overflow-y: auto;
  padding: 10px 15px;
`;

const CategoryTitle = styled.h4`
  margin: 15px 0 10px;
  font-size: 16px;
  color: #333;
`;

const IndicatorItem = styled.div`
  margin-bottom: 8px;
`;

const IndicatorRow = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 5px 0;
`;

const IndicatorLabel = styled.label`
  display: flex;
  align-items: center;
  cursor: pointer;
  
  input[type="checkbox"] {
    margin-right: 8px;
  }
`;

const SettingsButton = styled.button`
  background: none;
  border: none;
  cursor: pointer;
  color: #666;
  font-size: 16px;
  
  &:hover {
    color: #2962FF;
  }
`;

const SettingsPopup = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1100;
`;

const SettingsContent = styled.div`
  background-color: white;
  border-radius: 8px;
  width: 300px;
  max-width: 90%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  overflow: hidden;
`;

const SettingsHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 15px;
  border-bottom: 1px solid #eee;
`;

const SettingsTitle = styled.h4`
  margin: 0;
  font-size: 16px;
`;

const SettingsBody = styled.div`
  padding: 15px;
  max-height: 400px;
  overflow-y: auto;
`;

const SettingsFooter = styled.div`
  display: flex;
  justify-content: flex-end;
  padding: 10px 15px;
  border-top: 1px solid #eee;
`;

const SettingsButton2 = styled.button`
  padding: 8px 15px;
  margin-left: 10px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
  
  &:first-child {
    background-color: #2962FF;
    color: white;
    
    &:hover {
      background-color: #1E4FC4;
    }
  }
  
  &:last-child {
    background-color: #f5f5f5;
    color: #333;
    
    &:hover {
      background-color: #e0e0e0;
    }
  }
`;

const IndicatorSidebar = ({ isOpen, onClose, onSelectIndicator, selectedIndicators, onUpdateSettings, indicatorSettings }) => {
    const [showSettings, setShowSettings] = useState(false);
    const [selectedIndicator, setSelectedIndicator] = useState(null);
    
    const handleIndicatorClick = (indicator) => {
        onSelectIndicator(indicator.id);
    };

    const handleSettingsClick = (e, indicator) => {
        e.preventDefault();
        e.stopPropagation();
        setSelectedIndicator(indicator);
        setShowSettings(true);
    };

    const handleSettingsSave = (indicatorId, settings) => {
        onUpdateSettings(indicatorId, settings);
        setShowSettings(false);
    };

    return (
        <Sidebar className={isOpen ? 'open' : ''}>
            <SidebarHeader>
                <SidebarTitle>보조지표</SidebarTitle>
                <CloseButton onClick={onClose}>×</CloseButton>
            </SidebarHeader>
            <SearchContainer>
                <SearchInput type="text" placeholder="지표명 검색" />
            </SearchContainer>
            <IndicatorList>
                {Object.entries(categorizedIndicators).map(([category, items]) => (
                    <div key={category}>
                        <CategoryTitle>{category}</CategoryTitle>
                        {items.map(indicator => (
                            <IndicatorItem key={indicator.id}>
                                <IndicatorRow>
                                    <IndicatorLabel>
                                        <input
                                            type="checkbox"
                                            checked={selectedIndicators.includes(indicator.id)}
                                            onChange={() => handleIndicatorClick(indicator)}
                                        />
                                        {indicator.name}
                                    </IndicatorLabel>
                                    {indicator.hasSettings && selectedIndicators.includes(indicator.id) && (
                                        <SettingsButton 
                                            onClick={(e) => handleSettingsClick(e, indicator)}
                                        >
                                            ⚙️
                                        </SettingsButton>
                                    )}
                                </IndicatorRow>
                            </IndicatorItem>
                        ))}
                    </div>
                ))}
            </IndicatorList>

            {showSettings && selectedIndicator && (
                <IndicatorSettingsPopup 
                    indicator={selectedIndicator}
                    onClose={() => setShowSettings(false)}
                    onSave={handleSettingsSave}
                    currentSettings={indicatorSettings[selectedIndicator.id]}
                />
            )}
        </Sidebar>
    );
};

// 지표 설정 팝업 컴포넌트
const IndicatorSettingsPopup = ({ indicator, onClose, onSave, currentSettings }) => {
    // 현재 설정이 있으면 사용하고, 없으면 기본 설정 사용
    const [settings, setSettings] = useState(() => {
        return currentSettings || { ...indicator.component.defaultSettings };
    });

    const handleSettingsChange = (newSettings) => {
        setSettings(newSettings);
    };

    const handleSave = () => {
        onSave(indicator.id, settings);
    };

    // 팝업 외부 클릭 시 닫기
    const handleOutsideClick = (e) => {
        if (e.target === e.currentTarget) {
            onClose();
        }
    };

    // 지표별 설정 컴포넌트 렌더링
    const SettingsComponent = indicator.component.SettingsComponent;

    return (
        <SettingsPopup onClick={handleOutsideClick}>
            <SettingsContent>
                <SettingsHeader>
                    <SettingsTitle>{indicator.name} 설정</SettingsTitle>
                    <CloseButton onClick={onClose}>×</CloseButton>
                </SettingsHeader>
                <SettingsBody>
                    {SettingsComponent && (
                        <SettingsComponent 
                            settings={settings} 
                            onSettingsChange={handleSettingsChange} 
                        />
                    )}
                </SettingsBody>
                <SettingsFooter>
                    <SettingsButton2 className="confirm-button" onClick={handleSave}>확인</SettingsButton2>
                    <SettingsButton2 className="cancel-button" onClick={onClose}>취소</SettingsButton2>
                </SettingsFooter>
            </SettingsContent>
        </SettingsPopup>
    );
};

export default IndicatorSidebar; 