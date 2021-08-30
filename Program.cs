using System;
using System.Timers;
using System.Threading;

// See also Multi Media Timers if you want better resolution:
// https://stackoverflow.com/questions/24839105/high-resolution-timer-in-c-sharp
// Will run only on Windows

namespace TimerTest
{

    class MainLoop
    {
        public void run()
        {
            var t = new IntervalTimer(1, Handler);
            while (true)
            {
                for (var a = 0; a < 1000000; a++)
                {
                    // Busy Loop to stress system
                    if (a==12345) { a++;}
                }
            }
        }

        private void Handler(IntervalTimer t)
        {
            if (t.Statistics.IntervalCount % 300 == 0)
            {
                Console.WriteLine("{0:HH:mm:ss.fff} {1}", DateTime.Now, t.Statistics);
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
