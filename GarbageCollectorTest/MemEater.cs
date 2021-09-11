/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Threading;


namespace TimerTest
{
    class MemEater
    {
        private char[] _chunk;
        public MemEater(int size)
        {
            GCObjectCounter.inc();
            _chunk = new char[size];
            for (int i = 0; i < size; i++)
            {
                _chunk[i] = (char)i;
            }
        }

        ~MemEater()
        {
            //ACHTUNG: Diese Methode wird vom Garbage Collector scheinbar parallel aufgerufen!
            //GCStats.dec muss Thread-safe sein ein simples _eaterCount-- funktioniert nicht!!!
            GCObjectCounter.dec();
        }
    }

}
