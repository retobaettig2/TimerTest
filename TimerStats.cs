/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;

namespace TimerTest
{

    public class TimerStats
    {
        public Histogram Hist { get; private set; }
        public long MinInterval { get; private set; }
        public long MaxInterval { get; private set; }

        public long IntervalCount { get; private set; }
        public long ReentranceCount { get; private set; }

        private long _meanAccumulator;

        public TimerStats()
        {
            Hist = new Histogram(Config.binBoundaries);
            ResetStats();
        }
        public float MeanInterval
        {
            get
            {
                if (IntervalCount > 0)
                {
                    return (float)_meanAccumulator / IntervalCount;
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

        public void AddReentrance()
        {
            ReentranceCount++;
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
            return String.Format("{0}: Min interval: {1}ms, Max interval: {2}ms,  Average: {3:0.0}ms/{4:0.00}Hz, Reentered {5} times",
                base.ToString(),
                MinInterval,
                MaxInterval,
                MeanInterval,
                1000/MeanInterval,
                ReentranceCount);
        }
    }
}