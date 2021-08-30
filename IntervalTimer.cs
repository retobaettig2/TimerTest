/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Timers;

namespace TimerTest
{
    public class IntervalTimer
    {
        private System.Timers.Timer _timer;
        private long _lastTimeStamp;
        public delegate void Del(IntervalTimer timer);
        public Del Handler { private get; set; }
        public TimerStats Statistics { get; private set; }

        public IntervalTimer(int msDelay, Del handler = null)
        {
            Statistics = new TimerStats();
            Handler = handler;
            _timer = new System.Timers.Timer(msDelay);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _lastTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _timer.Enabled = true;
        }

        ~IntervalTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var diff = now - _lastTimeStamp;
            _lastTimeStamp = now;

            Statistics.AddInterval(diff);

            if (Handler!=null) {
                Handler(this);
            }

        }

        public override string ToString()
        {
            return String.Format("{0}: Min interval: {1}ms, Max interval: {2}ms,  Average: {3}ms", 
                base.ToString(),
                Statistics.MinInterval, 
                Statistics.MaxInterval, 
                Statistics.MeanInterval);
        }

    }
}
