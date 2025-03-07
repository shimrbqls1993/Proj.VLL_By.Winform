using Proj.VVL.Interfaces.DataInventoryHandlers.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.PubSub.Subscribers;
using LiveChartsCore.Defaults;

namespace Proj.VVL.Interfaces.DataInventoryHandlers
{
    public class JsonHandler : IJsonHandler
    {
        private static readonly Dictionary<string, Mutex> fileMutexs = new Dictionary<string, Mutex>();
        public void WriteJsonToFile(CANDLE_STICK_DEF[] data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

            // 파일이 없으면 생성하고, JSON 데이터를 씀
            File.WriteAllText(filePath, jsonData);

            Console.WriteLine("JSON 파일이 생성되었습니다.");
        }

        public void WriteJsonToFile(CANDLE_DATA_DEF data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            Mutex mutex = GetMutexForFilePath(filePath);
            
            try
            {
                mutex.WaitOne();
                // 파일이 없으면 생성하고, JSON 데이터를 씀
                File.WriteAllText(filePath, jsonData);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private Mutex GetMutexForFilePath(string filePath)
        {
            Mutex mutex;
            lock (fileMutexs)
            {
                if(!fileMutexs.TryGetValue(filePath, out mutex))
                {
                    mutex = new Mutex();
                    fileMutexs.Add(filePath, mutex);
                }
            }
            return mutex;
        }

        public T ReadJsonFromFile<T>(string filePath)
        {
            // 파일이 존재하는지 확인
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return default(T);
            }
        }
    }
}
