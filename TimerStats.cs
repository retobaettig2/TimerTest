/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;

namespace TimerTest
{

    public class Histogram {

        private long[] _binBoundaries;
        private long[] _bins;
        private long _count;
        public Histogram(long[] BinBoundaries) {
            _binBoundaries = BinBoundaries;
            _bins = new long[BinBoundaries.Length+1];
        }

        public void add(long measurement) {
            int i=0;
            while ((i<_binBoundaries.Length) && (measurement > _binBoundaries[i])) {
                i++;
            }
            _bins[i]++;
            _count++;
        }

        public override string ToString()
        {
            string s="";
            
            for (int i=0; i<_binBoundaries.Length; i++) {
                s+="<"+_binBoundaries[i]+"ms\t";
            }
            s+= ">"+_binBoundaries[_binBoundaries.Length-1]+"\n";
            for (int i=0; i<_bins.Length; i++) {
                s+=String.Format("{0:0.0}%\t",(double)_bins[i]/_count*100);
            }
            s+= "\n";
            for (int i=0; i<_bins.Length; i++) {
                s+=String.Format("{0}\t",_bins[i]);
            }

            return s;
        }
    }
    public class TimerStats
    {
        public Histogram Hist { get; private set;}
        public long MinInterval { get; private set; }
        public long MaxInterval { get; private set; }

        public long IntervalCount { get; private set; }

        private long _meanAccumulator;
        
        public TimerStats() {
            Hist = new Histogram(new long[] { 5, 10, 15, 20, 30, 50, 100, 1000 });
            ResetStats();
        }
        public float MeanInterval
        {
            get
            {
                if (IntervalCount > 0)
                {
                    return _meanAccumulator / IntervalCount;
                }
                return 0;
            }
        }

        public void ResetStats()
        {
            MinInterval = Int64.MaxValue;
            MaxInterval = 0;
            IntervalCount = 0;
            _meanAccumulator = 0;
        }

        public void AddInterval(long interval)
        {
            _meanAccumulator += interval;
            IntervalCount++;

            MaxInterval = Math.Max(interval, MaxInterval);
            MinInterval = Math.Min(interval, MinInterval);

            Hist.add(interval);
        }

        public override string ToString()
        {
            return String.Format("{0}: Min interval: {1}ms, Max interval: {2}ms,  Average: {3}ms", 
                base.ToString(),
                MinInterval, 
                MaxInterval, 
                MeanInterval);
        }
    }
}