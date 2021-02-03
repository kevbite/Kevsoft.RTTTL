using System;

namespace Kevsoft.RTTTL
{
    public sealed class ConsoleBeepPlayer : FrequencyRtttlPlayer
    {
        protected override void Play(double frequency, TimeSpan duration)
        {
            Console.Beep((int)frequency, duration.Milliseconds);
        }
    }
}