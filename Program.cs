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

    class MainLoop
    {
        private GarbageCollectorTest gc;
        public void run()
        {
            gc = new GarbageCollectorTest(1000, 1024*1024);
            
            var t = new IntervalTimer(1, Handler);
            while (true)
            {
                gc.Iterate();
            }
        }

        private void Handler(IntervalTimer t)
        {
            if (t.Statistics.IntervalCount % 300 == 0)
            {
                Console.WriteLine("{0:HH:mm:ss.fff} {1} \n    {2}", DateTime.Now, t.Statistics, gc);
                Console.WriteLine(t.Statistics.Hist+"\n");
            }
        }
    }

    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Testing Dotnet Timer frequency and accuracy");
            Console.WriteLine("Press <ctrl>-<c> to abort.");
            new MainLoop().run(); 
        }

    }
}
