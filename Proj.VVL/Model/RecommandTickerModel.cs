using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Proj.VVL.Interfaces.KiwoomHandlers.RecommandTickerHandler;

namespace Proj.VVL.Model
{
    public class RecommandTickerModel
    {
        public class Ticker
        {
            public Ticker(string name, string code, bool isActive = true)
            {
                Name = name;
                Code = code;
                IsActive = isActive;
            }

            public string Name { get; set; }
            public string Code { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
