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
        public void run(int objectcount, int objectsize)
        {
            if (objectcount>0) {
                gc = new GarbageCollectorTest(objectcount, objectsize);
            }
                        
            var t = new IntervalTimer(1, Handler);
            //Busy Loop to stress system
            while (true)
            {
                if (objectcount>0) {
                    gc.Iterate();
                }
            }
        }

        private void Handler(IntervalTimer t)
        {
            if (t.Statistics.IntervalCount % 300 == 0)
            {
                Console.WriteLine("{0:HH:mm:ss.fff} {1}", DateTime.Now, t.Statistics);
                if (gc!=null) {
                    Console.WriteLine(gc);
                }
                Console.WriteLine(t.Statistics.Hist+"\n");
            }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Testing Dotnet Timer frequency and accuracy");
            Console.WriteLine("Usage: TimerTest [objectcount] [objectsize]");
            Console.WriteLine("  if object count and size are specified, the program \nis allocating and freeing objects of the given size as fast as possible");
            Console.WriteLine("Press <ctrl>-<c> to abort.");

            int objectcount, objectsize;
            if (args.Length<1 || !int.TryParse(args[0], out objectcount)) {
                objectcount = 0;
            }
            if (args.Length<2 || !int.TryParse(args[1], out objectsize)) {
                objectsize = 0;
            }
                        
            Console.WriteLine("objectcount = {0}, objectsize = {1}", objectcount, objectsize);
            new MainLoop().run(objectcount, objectsize); 
        }

    }
}
