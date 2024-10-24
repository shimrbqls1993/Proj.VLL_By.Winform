using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomHandlers.Abstractions
{
    internal interface IScreenNumberHandler
    {
        public int GetScreenNumber();
        public void ReleaseScreenNumber(int releaseNumber);
    }
}
