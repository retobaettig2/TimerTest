/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Threading;


namespace TimerTest
{
    static class GCStats {
        static long _objectCount;
        public static void inc() {
            Interlocked.Increment(ref _objectCount);
        }
        public static void dec() {
            //Muss Thread-Safe sein, weil es vom Garbage-Collector parallel ausgeführt wird!
            //_eaterCount-- ist NICHT Thread-Safe!
            Interlocked.Decrement(ref _objectCount);;
        }

        public static long getActiveCount() {
            return _objectCount;
        }
    }

    class MemEater {
        private char[] _chunk;
        public MemEater(int size) {
            GCStats.inc();
            _chunk = new char[size];
            for (int i=0; i<size; i++) {
                _chunk[i]=(char)i;
            }
        }

        ~MemEater() {
            //ACHTUNG: Diese Methode wird vom Garbage Collector scheinbar parallel aufgerufen!
            //GCStats.dec muss Thread-safe sein ein simples _eaterCount-- funktioniert nicht!!!
            GCStats.dec();
        }
    }
    public class GarbageCollectorTest {
        private MemEater[] _objects;
        private int _numObjects;
        private int _objectSize;
        private long _allocated;
        private long _freed;

        public GarbageCollectorTest(int numObjects, int objectSize=1024*1024) {
            _objects = new MemEater[numObjects];
            _numObjects = numObjects;
            _objectSize = objectSize;
        }

        public void Iterate() {
            for (int i=0; i<_numObjects; i++) {
                // Old Eaters are overwritten => they should be collected by the GC eventually
                if (_objects[i]!=null) { _freed += _objectSize; }
                _objects[i] = new MemEater(_objectSize);
                _allocated += _objectSize;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: Allocated: {1}MB, Freed: {2}MB, Not collected: {3}MB", 
                base.ToString(),
                _allocated/1024/1024, 
                _freed/1024/1024, 
                GCStats.getActiveCount()*_objectSize/1024/1024);
        }

    }
    
}
