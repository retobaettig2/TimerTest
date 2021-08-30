/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Threading;


namespace TimerTest
{
    static class GCStats {
        static long _eaterCount;
        public static void inc() {
            Interlocked.Increment(ref _eaterCount);
        }
        public static void dec() {
            //Muss Thread-Safe sein, weil es vom Garbage-Collector parallel ausgeführt wird!
            //_eaterCount-- ist NICHT Thread-Safe!
            Interlocked.Decrement(ref _eaterCount);;
        }

        public static long getActiveCount() {
            return _eaterCount;
        }
    }

    class MemEater {
        private int[] _chunk;
        public MemEater(int size) {
            GCStats.inc();
            _chunk = new int[size];
            for (int i=0; i<size; i++) {
                _chunk[i]=i;
            }
        }

        ~MemEater() {
            //ACHTUNG: Diese Methode wird vom Garbage Collector scheinbar parallel aufgerufen!
            //GCStats.dec muss Thread-safe sein ein simples _eaterCount-- funktioniert nicht!!!
            GCStats.dec();
        }
    }
    public class GarbageCollectorTest {
        private MemEater[] _eaters;
        private int _numEaters;
        private int _eaterSize;
        private long _allocated;
        private long _freed;

        public GarbageCollectorTest(int numEaters, int eaterSize=1024*1024) {
            _eaters = new MemEater[numEaters];
            _numEaters = numEaters;
            _eaterSize = eaterSize;
        }

        public void Iterate() {
            for (int i=0; i<_numEaters; i++) {
                // Old Eaters are overwritten => they should be collected by the GC eventually
                if (_eaters[i]!=null) { _freed += _eaterSize; }
                _eaters[i] = new MemEater(_eaterSize);
                _allocated += _eaterSize;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: Allocated: {1}MB, Freed: {2}MB, Not collected: {3}MB", 
                base.ToString(),
                _allocated/1024/1024, 
                _freed/1024/1024, 
                GCStats.getActiveCount()*_eaterSize/1024/1024);
        }

    }
    
}
