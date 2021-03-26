using System.Runtime.Versioning;
using Kevsoft.RTTTL.Console;

// ReSharper disable once CheckNamespace
namespace Kevsoft.RTTTL
{
    /// <summary>
    /// Rtttl Extensions for playing tunes via passive buzzer with a PWM Channel.
    /// </summary>
    public static class RtttlExtensions
    {
        /// <summary>
        /// Play with console beep.
        /// </summary>
        /// <param name="rtttl">Rtttl</param>
        [SupportedOSPlatform("windows")]
        public static void PlayWithConsoleBeep(this Rtttl rtttl)
        {
            rtttl.Play(new ConsoleBeepPlayer());
        }
    }
}