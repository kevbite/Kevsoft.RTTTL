using Kevsoft.RTTTL.Device.Gpio;
using System.Device.Pwm;

// ReSharper disable once CheckNamespace
namespace Kevsoft.RTTTL
{
    public static class RtttlPwmChannelExtensions
    {
        public static void PlayWithPwmChannel(this Rtttl rtttl, int chip, int channel)
        {
            rtttl.Play(new PwmChannelPlayer(chip, channel));
        }

        public static void PlayWithPwmChannel(this Rtttl rtttl, PwmChannel pwmChannel)
        {
            rtttl.Play(new PwmChannelPlayer(pwmChannel));
        }
    }
}