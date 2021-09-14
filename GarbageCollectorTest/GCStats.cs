/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Threading;


namespace TimerTest
{
    static class GCObjectCounter
    {
        static long _objectCount;
        public static void inc()
        {
            Interlocked.Increment(ref _objectCount);
        }
        public static void dec()
        {
            //Muss Thread-Safe sein, weil es vom Garbage-Collector parallel ausgeführt wird!
            //_eaterCount-- ist NICHT Thread-Safe!
            Interlocked.Decrement(ref _objectCount); ;
        }

        public static long getActiveCount()
        {
            return _objectCount;
        }
    }

}
