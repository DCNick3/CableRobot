using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CableRobot
{
    /// <summary>
    /// Helper class to make some peace of code be executed specified times per secound
    /// </summary>
    public class Rate
    {
        // Well, Windows scheduler can't provide us with needed time precision, so we would use busy sleep
        private static readonly bool UseBusySleep = Environment.OSVersion.Platform == PlatformID.Win32NT;

        private readonly int _windowSize;
        private readonly TimeSpan _period;
        private DateTime _lastTick;
        
        private readonly Queue<TimeSpan> _tickTimes = new Queue<TimeSpan>();
        private TimeSpan _windowTime = TimeSpan.Zero;

        public Rate(double desiredRate) : this(TimeSpan.FromSeconds(1.0 / desiredRate))
        {}

        public Rate(TimeSpan period) : this(period, TimeSpan.FromSeconds(1))
        {}
        
        public Rate(TimeSpan period, TimeSpan windowSize)
        {
            _period = period;
            _lastTick = DateTime.Now;
            _windowSize = (int) Math.Round(windowSize.TotalSeconds / period.TotalSeconds);
        }

        public TimeSpan Period => _period;
        public double WantedRate => 1.0 / _period.TotalSeconds;
        public double RealRate => Math.Min(_windowSize, _tickTimes.Count) / _windowTime.TotalSeconds;

        private void RegisterTick(TimeSpan diff)
        {
            _tickTimes.Enqueue(diff);
            _windowTime += diff;
            if (_tickTimes.Count > _windowSize)
                _windowTime -= _tickTimes.Dequeue();
        }
        
        
        private void BusySleep(double ms)
        {
            var dt = DateTime.Now + TimeSpan.FromMilliseconds(ms);
            while (DateTime.Now < dt)
                ;
        }

        private void SleepMs(double ms)
        {
            if (ms < 1.0)
                return;
            
            if (UseBusySleep)
                BusySleep(ms);
            else
                Thread.Sleep((int)Math.Round(ms));
        }

        public void Sleep()
        {
            var elapsed = DateTime.Now - _lastTick;
            var sleepTime = Math.Max((_period - elapsed).TotalMilliseconds, 0);
            SleepMs(sleepTime); // Use some magic~
            RegisterTick(DateTime.Now - _lastTick);
            _lastTick = DateTime.Now;
        }
    }
}