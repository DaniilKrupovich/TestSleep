using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSleep
{
    public interface ITimer
    {
        void Start(int intervalMs, Action callback);
        void Stop();
    }
}
