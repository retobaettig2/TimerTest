/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Timers;
using System.Threading;

namespace TimerTest
{
    public class IntervalTimer
    {
        private System.Timers.Timer _timer;
        private long _timerRunning = 0;
        private long _lastTimeStamp;
        public delegate void Del(IntervalTimer timer);
        Action<IntervalTimer> _handler;
        public TimerStats Statistics { get; private set; }

        public IntervalTimer(int msDelay, Action<IntervalTimer> handler = null)
        {
            Statistics = new TimerStats();
            _handler = handler;
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
            // prevent reentrance
            long otherRunning = Interlocked.CompareExchange(ref _timerRunning, 1, 0);
            if (otherRunning == 1)
            {
                Statistics.AddReentrance();
                return;
            }

            try
            {
                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var diff = now - _lastTimeStamp;
                _lastTimeStamp = now;

                Statistics.AddInterval(diff);

                if (_handler != null)
                {
                    _handler(this);
                }

            }
            finally
            {
                // prevent reentrance
                Interlocked.Exchange(ref _timerRunning, 0);
            }

        }

        public override string ToString()
        {
            return Statistics.ToString();
        }

    }
}
