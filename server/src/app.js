const express = require('express');
const cors = require('cors');
const candlestickRoutes = require('./routes/candlestick.routes');

const app = express();

// CORS 설정
app.use(cors());
app.use(express.json());

// 라우트 설정
app.use('/api/candlesticks', candlestickRoutes);

// 서버 시작
const PORT = process.env.PORT || 3001;
app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
