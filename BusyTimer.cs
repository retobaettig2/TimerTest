/*
Dotnet Timer Tests
29.8.21, Reto Bättig
*/

using System;
using System.Timers;
using System.Threading;

namespace TimerTest
{
    public class BusyTimer
    {
        private long _timerRunning = 0;
        private long _lastTimeStamp;
        private long _msLoopCount = 0;
        private int _msDelay = 0;

        private bool _threadStop = false;
        Action<Object> _handler;
        public TimerStats Statistics { get; private set; }

        public BusyTimer(int msDelay, Action<Object> handler = null)
        {
            Statistics = new TimerStats();
            _handler = handler;
            _msDelay = msDelay;
            Calibrate();
            _lastTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            (new Thread(BusyThread)).Start();
        }

        private void BusyThread()
        {
            long TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("Busy thread started");
            while (!_threadStop)
            {
                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() - TimeStamp < _msDelay)
                {
                    Sleep100uSeconds();
                }
                TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                OnTimedEvent();
            }
            Console.WriteLine("Busy thread stopped");
        }

        private void Sleep100uSeconds()
        {
            for (long i = 0; i < _msLoopCount/10; i++)
            {
                // Busy sleep for ~100 microseconds
            }
        }

        private void Calibrate()
        {
            long loopCount = 100000;
            long loopMs = 0;
            long before;

            Console.WriteLine("Calibrating busy wait timer...");
            while (loopMs < 1000)
            {
                loopCount *= 10;
                before = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                for (long i = 0; i < loopCount; i++)
                {
                    // do nothing
                }
                loopMs = DateTimeOffset.Now.ToUnixTimeMilliseconds() - before;
            }
            _msLoopCount = loopCount / loopMs;
            Console.WriteLine($"Calibration done, {_msLoopCount} Loops/ms");
        }

        ~BusyTimer()
        {
            _threadStop = true;
            Console.WriteLine("Busy thread stopping");
        }

        private void OnTimedEvent()
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
            return $"{Statistics}\n{Statistics.Hist}";
        }

    }
}
