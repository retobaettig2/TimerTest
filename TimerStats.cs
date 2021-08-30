using System;
using System.Timers;
using System.Threading;

namespace TimerTest
{
    public class TimerStats
    {
        public long MinInterval { get; private set; }
        public long MaxInterval { get; private set; }

        public long IntervalCount { get; private set; }

        private long _meanAccumulator;
        
        public TimerStats() {
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