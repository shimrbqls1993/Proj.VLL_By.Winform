const express = require('express');
const router = express.Router();
const candlestickController = require('../controllers/candlestick.controller');

router.get('/:code/:timeFrame', candlestickController.getCandleStickData);

module.exports = router;
