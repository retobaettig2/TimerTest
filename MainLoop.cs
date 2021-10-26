/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/
using System;
using System.Runtime.InteropServices;

namespace TimerTest
{

    class MainLoop
    {
        private GarbageCollectorTest _gc;
        private DateTime _lastOutput = DateTime.MinValue;
        private UInt32 timerId = 0;

        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        public static extern uint TimeBeginPeriod(uint uMilliseconds);

        // TODO: kill timer event
        [DllImport("winmm.dll", SetLastError = true, EntryPoint = "timeKillEvent")]
        internal static extern void TimeKillEvent(UInt32 uTimerId);

        public void run()
        {
            _gc = new GarbageCollectorTest(Config.objectCount, Config.objectSize);

            if (Config.threadMode)
            {
               new BusyTimer(Config.timerDelayms, Handler);
            } 
            else if (Config.mmTimerMode) {
                timerId = TimeBeginPeriod((uint)Config.timerDelayms);
                new HiResTimer(Config.timerDelayms, Handler);
            }
            else
            {
               new IntervalTimer(Config.timerDelayms, Handler);
            }

            //Busy Loop to stress system
            while (true)
            {
                _gc.Iterate();
            }
        }

        private void Handler(Object t)
        {
            if (ItsTimeToUpdateOutput())
            {
                Console.WriteLine(_gc);
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {t}\n");
            }
        }

        private bool ItsTimeToUpdateOutput()
        {
            double _timeSinceLastOutput = DateTime.Now.Subtract(_lastOutput).TotalSeconds;
            if (_timeSinceLastOutput > Config.updateTimeSeconds)
            {
                _lastOutput = DateTime.Now;
                return true;
            }
            return false;
        }

    }

}
