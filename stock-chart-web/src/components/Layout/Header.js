import React from 'react';
import styled from 'styled-components';
import SearchBar from '../SearchBar';

const HeaderContainer = styled.header`
    width: 100%;
    height: 60px;
    background-color: #fff;
    border-bottom: 1px solid #e0e0e0;
    display: flex;
    align-items: center;
    padding: 0 20px;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
`;

const Logo = styled.div`
    font-size: 20px;
    font-weight: bold;
    color: #ff1744;
    margin-right: 20px;
`;

const Header = ({ onSelectStock }) => {
    return (
        <HeaderContainer>
            <Logo>Stock Chart</Logo>
            <SearchBar onSelectStock={onSelectStock} />
        </HeaderContainer>
    );
};

export default Header; 