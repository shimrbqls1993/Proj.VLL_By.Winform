const { spawn } = require('child_process');
const path = require('path');

console.log('Starting Stock Chart Application...');

// 서버 실행
const server = spawn('npm', ['run', 'server'], {
    cwd: path.join(__dirname),
    shell: true
});

// 클라이언트 실행
const client = spawn('npm', ['run', 'client'], {
    cwd: path.join(__dirname),
    shell: true
});

server.stdout.on('data', (data) => {
    console.log(`Server: ${data}`);
});

client.stdout.on('data', (data) => {
    console.log(`Client: ${data}`);
});

server.stderr.on('data', (data) => {
    console.error(`Server Error: ${data}`);
});

client.stderr.on('data', (data) => {
    console.error(`Client Error: ${data}`);
});

process.on('SIGINT', () => {
    server.kill();
    client.kill();
    process.exit();
}); 