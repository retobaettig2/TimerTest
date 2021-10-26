using System;
using System.Threading;

namespace TimerTest
{
    class HiResTimer
    {
        private MTimer.Timer _timer;
        private long _timerRunning = 0;
        private long _lastTimeStamp;
        public delegate void Del(IntervalTimer timer);
        Action<HiResTimer> _handler;
        public TimerStats Statistics { get; private set; }

        public HiResTimer(int msDelay, Action<Object> handler = null)
        {
            Statistics = new TimerStats();
            _handler = handler;
            _timer = new MTimer.Timer(msDelay);
            _timer.Elapsed += OnTimedEvent;
            _lastTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _timer.Start();
        }

        private void OnTimedEvent(object sender, EventArgs e)
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

        ~HiResTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        public override string ToString()
        {
            return $"{Statistics}\n{Statistics.Hist}";
        }
    }
}
