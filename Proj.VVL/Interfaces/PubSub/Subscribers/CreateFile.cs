using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.PubSub.Subscribers
{
    public class CreateFile
    {
        string _code = string.Empty;
        public CreateFile(string code)
        {
            _code = code;
        }
    }
}
