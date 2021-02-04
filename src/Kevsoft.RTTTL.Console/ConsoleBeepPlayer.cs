using System;
using System.Threading;

namespace Kevsoft.RTTTL.Console
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
                System.Console.Beep((int) frequency, (int)duration.TotalMilliseconds);
            }
        }
    }
}