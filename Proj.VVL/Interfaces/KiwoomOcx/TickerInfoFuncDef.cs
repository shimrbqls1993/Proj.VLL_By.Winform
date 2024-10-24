using AxKHOpenAPILib;
using Proj.VVL.Interfaces.KiwoomOcx.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomOcx
{
    internal class TickerInfoFuncDef : ITickerInfoHandle
    {
        public AxKHOpenAPI OcxObject;

        public TickerInfoFuncDef(AxKHOpenAPI OcxObjBind)
        {
            OcxObject = OcxObjBind;
        }
    }
}
