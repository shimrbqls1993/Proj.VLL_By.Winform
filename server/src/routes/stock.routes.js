const express = require('express');
const router = express.Router();
const stockController = require('../controllers/stock.controller');

// 종목 검색 API
router.get('/search', stockController.searchStocks);

// 종목 데이터 가져오기 API
router.get('/fetch/:code', stockController.fetchStockData);

// 종목 정보 가져오기 API
router.get('/info/:code', stockController.getStockInfo);

module.exports = router; 