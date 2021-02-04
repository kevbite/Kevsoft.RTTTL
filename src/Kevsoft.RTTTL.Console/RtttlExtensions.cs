using Kevsoft.RTTTL.Console;

// ReSharper disable once CheckNamespace
namespace Kevsoft.RTTTL
{
    public static class RtttlExtensions
    {
        public static void PlayWithConsoleBeep(this Rtttl rtttl)
        {
            rtttl.Play(new ConsoleBeepPlayer());
        }
    }
}