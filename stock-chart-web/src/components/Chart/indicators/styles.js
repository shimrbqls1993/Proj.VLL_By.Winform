import styled from 'styled-components';

// 지표 설정 UI에 사용할 공통 스타일 컴포넌트
export const SettingsContainer = styled.div`
  padding: 10px;
  font-size: 14px;
`;

export const SettingTitle = styled.div`
  font-weight: bold;
  margin-bottom: 10px;
  padding-bottom: 5px;
  border-bottom: 1px solid #eee;
`;

export const SettingRow = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 15px;
`;

export const SettingLabel = styled.div`
  color: #333;
`;

export const SettingSelect = styled.select`
  width: 80px;
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background-color: white;
  font-size: 14px;
`;

export const SettingInput = styled.input`
  width: 80px;
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background-color: white;
  font-size: 14px;
  text-align: left;
`;

export const SettingCheckbox = styled.input.attrs({ type: 'checkbox' })`
  margin-right: 5px;
`;

export const SettingButton = styled.button`
  padding: 5px 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background-color: ${props => props.active ? '#2962FF' : 'white'};
  color: ${props => props.active ? 'white' : '#333'};
  font-size: 14px;
  cursor: pointer;
  margin-right: 5px;
  
  &:hover {
    background-color: ${props => props.active ? '#1E4FC4' : '#f5f5f5'};
  }
`;

export const ButtonGroup = styled.div`
  display: flex;
  margin-bottom: 15px;
`; 