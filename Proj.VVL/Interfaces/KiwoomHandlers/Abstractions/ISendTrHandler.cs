using Proj.VVL.Interfaces.KiwoomOcx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomHandlers.Abstractions
{
    internal interface ISendTrHandler
    {
        public ERROR_CODE_DEF Send(Func<ERROR_CODE_DEF> func);
    }
}
