using System;
using System.Threading;

namespace Kevsoft.RTTTL
{
    public sealed class ConsoleBeepPlayer : FrequencyRtttlPlayer
    {
        protected override void Play(double frequency, TimeSpan duration)
        {
            if (frequency == 0)
            {
                Thread.Sleep(duration);
            }
            else
            {
                Console.Beep((int) frequency, duration.Milliseconds);
            }
        }
    }
}