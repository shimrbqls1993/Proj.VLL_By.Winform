using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Interfaces.KiwoomHandlers
{
    public class ScreenNumberHandler : IScreenNumberHandler
    {
        public enum STATE
        {
            RELEASE,
            LOCK
        }

        const int START_SCREEN_NUMBER = 100;
        const int LAST_SCREEN_NUMBER = START_SCREEN_NUMBER + 200;
        public Dictionary<int, STATE> SCREEN_NUMBER = new Dictionary<int, STATE>();
        static Mutex mutex = new Mutex();

        public ScreenNumberHandler()
        {
            for (int i = START_SCREEN_NUMBER; i < LAST_SCREEN_NUMBER; i++)
            {
                SCREEN_NUMBER.Add(i, STATE.RELEASE);
            }
        }

        public int GetScreenNumber()
        {
            mutex.WaitOne();

            for (int i = START_SCREEN_NUMBER; i < LAST_SCREEN_NUMBER; i++)
            {
                if (SCREEN_NUMBER[i] == STATE.RELEASE)
                {
                    SCREEN_NUMBER[i] = STATE.LOCK;
                    mutex.ReleaseMutex();
                    return i;
                }
            }

            mutex.ReleaseMutex();
            return 0;
        }

        public void ReleaseScreenNumber(int releaseNumber)
        {
            SCREEN_NUMBER[releaseNumber] = STATE.RELEASE;
        }
    }
}