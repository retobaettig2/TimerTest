/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;

namespace TimerTest
{

    public class Histogram
    {

        private long[] _binBoundaries;
        private long[] _bins;
        private long _count;
        public Histogram(long[] BinBoundaries)
        {
            _binBoundaries = BinBoundaries;
            _bins = new long[BinBoundaries.Length + 1];
        }

        public void add(long measurement)
        {
            int i = 0;
            while ((i < _binBoundaries.Length) && (measurement > _binBoundaries[i]))
            {
                i++;
            }
            _bins[i]++;
            _count++;
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < _binBoundaries.Length; i++)
            {
                s += $"<{_binBoundaries[i]}ms\t";
            }
            s += ">" + _binBoundaries[_binBoundaries.Length - 1] + "\n";
            for (int i = 0; i < _bins.Length; i++)
            {
                
                s += $"{(double)_bins[i] / _count * 100:0.0}%\t";
            }
            s += "\n";
            for (int i = 0; i < _bins.Length; i++)
            {
                s += $"{_bins[i]}\t";
            }

            return s;
        }
    }

}