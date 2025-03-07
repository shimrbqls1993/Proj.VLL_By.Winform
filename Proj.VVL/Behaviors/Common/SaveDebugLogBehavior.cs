using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GW_SKYBLUE_DC_WPF.Behaviors
{

    public class SaveDebugLogBehavior
    {
        private BufferBlock<string> _bufferBlock;
        private ActionBlock<string> _actionBlock;
        public static string LogDirPath = "";
        public static string CsvFileName = "";
        public const string LogFileExtended = ".csv";
        public static string CsvFilePath = Path.Combine(LogDirPath, DateTime.Now.ToString("yyyy-MM-dd") + "_" + CsvFileName);
        const string CsvHeader = "Time,Message,FuncName\n";
        public SaveDebugLogBehavior(string logDirPath, string logFileName)
        {
            LogDirPath = logDirPath;
            CsvFileName = logFileName + LogFileExtended;

            CheckLogDir();
            _bufferBlock = new BufferBlock<string>();
            _actionBlock = new ActionBlock<string>(async csvLine =>
            {
                if (!File.Exists(CsvFilePath))
                {
                    File.WriteAllText(CsvFilePath, CsvHeader);
                }
                using (StreamWriter writer = new StreamWriter(CsvFilePath, true))
                {
                    await writer.WriteLineAsync(csvLine);
                }
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            });

            _bufferBlock.LinkTo(_actionBlock);
        }

        private void CheckLogDir()
        {
            if (!Directory.Exists(LogDirPath))
            {
                Directory.CreateDirectory(LogDirPath);
            }
        }

        public void ManageCsvLogFile()
        {
            DateTime nowDate = DateTime.Now;
            TimeSpan timeSpan1Days = TimeSpan.FromDays(1);
            CsvFilePath = Path.Combine(LogDirPath, nowDate.ToString("yyyy-MM-dd") + "_" + CsvFileName);
            string[] DateTime2Str = new string[10];
            for(int i =0; i<DateTime2Str.Length; i++)
            {
                DateTime2Str[i] = nowDate.ToString("yyyy-MM-dd");
                nowDate -= timeSpan1Days;
            }

            string[] logFiles = Directory.GetFiles(LogDirPath);
            
            foreach (string logFile in logFiles)
            {
                string fileName = Path.GetFileName(logFile);
                string filePath = Path.Combine(LogDirPath, fileName);
                for(int i = 0; i <= DateTime2Str.Length; i++)
                {
                    if(i == DateTime2Str.Length)
                    {
                        File.Delete(filePath);
                        break;
                    }
                    if (fileName.Contains(DateTime2Str[i]))
                    {
                        break;
                    }
                }
            }
        }

        public void Log(string logData, [CallerMemberName] string callerName = "")
        {
            string logDataWithTime = $"{DateTime.Now.ToString("HH:mm:ss")},{logData},{callerName}";
#if DEBUG
            Debug.WriteLine(logData);
#endif
            _bufferBlock.Post(logDataWithTime);
        }

        public async Task CompleteAsync()
        {
            _bufferBlock.Complete();
            await _actionBlock.Completion;
        }
    }
}
