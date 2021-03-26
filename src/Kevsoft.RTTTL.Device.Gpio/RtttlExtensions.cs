using Kevsoft.RTTTL.Device.Gpio;
using System.Device.Pwm;

// ReSharper disable once CheckNamespace
namespace Kevsoft.RTTTL
{
    /// <summary>
    /// Rtttl Extensions for playing tunes via passive buzzer with a PWM Channel.
    /// </summary>
    public static class RtttlPwmChannelExtensions
    {
        /// <summary>
        /// Play With PWM Channel.
        /// </summary>
        /// <param name="rtttl">Ring Tone Transfer Language.</param>
        /// <param name="chip">The PWM chip number.</param>
        /// <param name="channel">The PWM channel number.</param>
        public static void PlayWithPwmChannel(this Rtttl rtttl, int chip, int channel)
        {
            rtttl.Play(new PwmChannelPlayer(PwmChannel.Create(chip, channel)));
        }

        /// <summary>
        /// Play With PWM Channel.
        /// </summary>
        /// <param name="rtttl">Ring Tone Transfer Language.</param>
        /// <param name="pwmChannel">PWM channel.</param>
        public static void PlayWithPwmChannel(this Rtttl rtttl, PwmChannel pwmChannel)
        {
            rtttl.Play(new PwmChannelPlayer(pwmChannel));
        }
    }
}