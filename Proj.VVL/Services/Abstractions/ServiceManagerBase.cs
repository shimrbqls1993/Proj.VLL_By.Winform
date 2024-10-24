using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Services.Abstractions
{
    public class ServiceManagerBase : IServiceManager
    {
        public Thread Handler;
        public bool IsRunning;

        public virtual void Start() 
        {
        }

        public virtual void Stop() 
        {

        }
    }
}
