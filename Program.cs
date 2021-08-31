/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/
using System;

// See also Multi Media Timers if you want better resolution:
// https://stackoverflow.com/questions/24839105/high-resolution-timer-in-c-sharp
// Will run only on Windows

namespace TimerTest
{

    // Simple compile time configuration instead of config file
    // ToDo: Move to config file
    public static class Config
    {
        public static int timerDelayms = 1;
        public static int updateTimeSeconds = 10;
        public static int objectCount = 0;
        public static int objectSize = 0;
        public static long[] binBoundaries = { 5, 10, 12, 14, 16, 18, 20, 30, 50, 100, 1000 };
    }
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
                Console.WriteLine("{0:HH:mm:ss.fff} {1}", DateTime.Now, t.Statistics);
                Console.WriteLine(_gc);
                Console.WriteLine(t.Statistics.Hist + "\n");
            }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            
            ParseArguments(args);
            Console.WriteLine("Usage: TimerTest [objectcount] [objectsize]");
            Console.WriteLine("   Testing Dotnet Timer frequency and accuracy");
            Console.WriteLine("   if object count and size are specified, the program");
            Console.WriteLine("   is allocating and freeing objects of the given size as fast as possible");
            Console.WriteLine("   Configuration: ");
            Console.WriteLine("       TimerDelay = {0}ms, UpdateTimeSeconds = {1}", Config.timerDelayms, Config.updateTimeSeconds);
            Console.WriteLine("       objectcount = {0}, objectsize = {1}", Config.objectCount, Config.objectSize);
            Console.WriteLine("Press <ctrl>-<c> to abort.");

            new MainLoop().run();
        }

        public static void ParseArguments(string[] args) {
            if (args.Length < 1 || !int.TryParse(args[0], out Config.objectCount))
            {
                Config.objectCount = 0;
            }
            if (args.Length < 2 || !int.TryParse(args[1], out Config.objectSize))
            {
                Config.objectSize = 0;
            }
        }

    }
}
