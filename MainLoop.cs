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

            var t = new IntervalTimer(Config.timerDelayms, Handler);
            //Busy Loop to stress system
            while (true)
            {
                _gc.Iterate();
            }
        }

        private void Handler(IntervalTimer t)
        {
            if (DateTime.Now.Subtract(_lastOutput).TotalSeconds > Config.updateTimeSeconds)
            {
                _lastOutput = DateTime.Now;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {t.Statistics}");
                Console.WriteLine(_gc);
                Console.WriteLine($"{t.Statistics.Hist}\n");
            }
        }
    }

}
