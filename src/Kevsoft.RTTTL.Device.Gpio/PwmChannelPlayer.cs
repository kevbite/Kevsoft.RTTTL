using System;
using System.Threading;
using System.Device.Pwm;

namespace Kevsoft.RTTTL.Device.Gpio
{
    public sealed class PwmChannelPlayer : FrequencyRtttlPlayer, IDisposable
    {
        private readonly PwmChannel _pwmChannel;

        public PwmChannelPlayer(PwmChannel pwmChannel) => _pwmChannel = pwmChannel;

        protected override void Play(double frequency, TimeSpan duration)
        {
            if (frequency == 0)
            {
                _pwmChannel.Stop();
            }
            else
            {
                _pwmChannel.Frequency = (int)frequency;
                _pwmChannel.Start();
            }

            Thread.Sleep(duration);
            _pwmChannel.Stop();
        }

        public void Dispose()
        {
            _pwmChannel?.Dispose();
        }
    }
}
