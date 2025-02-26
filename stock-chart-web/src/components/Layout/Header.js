import React from 'react';
import styled from 'styled-components';

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
`;

const SearchBar = styled.input`
    margin-left: 40px;
    padding: 8px 16px;
    border: 1px solid #ddd;
    border-radius: 4px;
    width: 300px;
    &:focus {
        outline: none;
        border-color: #2962FF;
    }
`;

const Header = () => {
    return (
        <HeaderContainer>
            <Logo>Stock Chart</Logo>
            <SearchBar placeholder="종목 검색" />
        </HeaderContainer>
    );
};

export default Header; 