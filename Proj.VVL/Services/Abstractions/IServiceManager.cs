using Microsoft.VisualBasic.FileIO;
using Proj.VVL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Abstractions
{
    public interface IServiceManager
    {
        void Start();
        void Stop();
    }
}
