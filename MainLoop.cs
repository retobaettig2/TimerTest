/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/
using System;

namespace TimerTest
{

    class MainLoop
    {
        private GarbageCollectorTest _gc;
        private DateTime _lastOutput = DateTime.MinValue;
        public void run()
        {
            _gc = new GarbageCollectorTest(Config.objectCount, Config.objectSize);

            //var t = new IntervalTimer(Config.timerDelayms, Handler);
            var bt = new BusyTimer(20, Handler);
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
