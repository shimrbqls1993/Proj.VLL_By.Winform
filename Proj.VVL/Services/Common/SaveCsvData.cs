using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Service.Common
{
    internal class SaveCsvData
    {
        Mutex mut;
        string fileName;
        string dirName;
        public SaveCsvData(string TickerName) 
        {
            mut = new Mutex();
            dirName = $"{Application.StartupPath}\\{DateTime.Now.ToString("yyyy")}";
            fileName = Path.Combine(dirName, $"{TickerName}_{DateTime.Now.ToString("MM.dd.HH")}.csv");
            Create();
        }

        void Create()
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        public void Save(string text)
        {
            Task.Factory.StartNew(() =>
            {
                mut.WaitOne();
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine(text);
                }
                mut.ReleaseMutex();
            });
        }
    }
}
