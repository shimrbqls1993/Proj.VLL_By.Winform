using Proj.VVL.Data;
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

        const string CommonDebugHeader = "Time,Message,FuncName";
        private static DefineLogObject CommonDebug = new DefineLogObject("Common", "CommonDebug", CommonDebugHeader, nameof(CommonDebug));
        const string ApiCallDebugHeader = "Time,Message,FuncName";
        private static DefineLogObject ApiCallDebug = new DefineLogObject("ApiCall", "ApiCallDebug", ApiCallDebugHeader, nameof(ApiCallDebug));
        const string TradingDebugHeader = "Time,Stock,BuySell,Price,Amount,Reason";
        private static DefineLogObject TradingDebug = new DefineLogObject("Trading", "TradingDebug", TradingDebugHeader, nameof(TradingDebug));


        public SaveDebugLogBehavior()
        {
            _bufferBlock = new BufferBlock<string>();
            _actionBlock = new ActionBlock<string>(async csvLine =>
            {
                try 
                {
                    if (csvLine.Contains(CommonDebug.Filter))
                    {
                        CommonDebug.WriteLogData(csvLine.Replace(CommonDebug.Filter, ""));
                    }
                    else if (csvLine.Contains(ApiCallDebug.Filter))
                    {
                        ApiCallDebug.WriteLogData(csvLine.Replace(ApiCallDebug.Filter, ""));
                    }
                    else if (csvLine.Contains(TradingDebug.Filter))
                    {
                        TradingDebug.WriteLogData(csvLine.Replace(TradingDebug.Filter, ""));
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            });

            _bufferBlock.LinkTo(_actionBlock);
        }

        public void CommonLog(string logData, [CallerMemberName] string callerName = "")
        {
            string logDataWithTime = $"{DateTime.Now.ToString("HH:mm:ss")},{logData},{callerName}";
#if DEBUG
            Debug.WriteLine(logData);
#endif
            _bufferBlock.Post(logDataWithTime);
        }

        public void ApiCallLog(string logData, [CallerMemberName] string callerName = "")
        {
            string logDataWithTime = $"{DateTime.Now.ToString("HH:mm:ss")},{logData},{callerName}";
#if DEBUG
            Debug.WriteLine(logData);
#endif
            _bufferBlock.Post(logDataWithTime);
        }

        public void TradingLog(string logData, [CallerMemberName] string callerName = "")
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

    public class DefineLogObject
    {

        public Mutex FileAccessMute = new Mutex();
        public string Filter = string.Empty;
        private string DirName = string.Empty;
        private string _fileName = string.Empty;
        private string CsvHeader = string.Empty;

        public DefineLogObject(string dirName, string fileName, string csvHeader, string nameofObj)
        {
            DirName = dirName;
            _fileName = fileName;
            CsvHeader = csvHeader + "\n";
            Filter = ","+ nameofObj;
            CheckDirExsit();
        }

        private void CheckDirExsit()
        {
            if (!Directory.Exists(Define.LOG_DIR_PATH))
            {
                Directory.CreateDirectory(Define.LOG_DIR_PATH);
            }
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
        }

        public void WriteLogData(string logData)
        {
            FileAccessMute.WaitOne();
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, CsvHeader);
            }
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine(logData);
            }
            FileAccessMute.ReleaseMutex();
        }

        public string FileName
        {
            get { return $"{DateTime.Now.ToString("yyyy-MM-dd")}_{_fileName}.csv"; }
        }
        public string DirPath
        {
            get { return Path.Combine(Define.LOG_DIR_PATH, DirName); }
        }
        public string FilePath
        {
            get { return Path.Combine(Define.LOG_DIR_PATH,DirName,FileName); }
        }

    }
}
