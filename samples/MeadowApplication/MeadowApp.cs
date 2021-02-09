using Meadow;
using Meadow.Devices;
using System;
using System.Threading;
using Kevsoft.RTTTL;
using Meadow.Hardware;

namespace MeadowApplication
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private readonly Random _random = new Random();
        private readonly IPwmPort _pwmBuzzer;

        public MeadowApp()
        {
            _pwmBuzzer = Device.CreatePwmPort(Device.Pins.D07);
            PlayTunes();
        }

        private static readonly string[] Songs = new string[]
        {
            "Axel F:d=4,o=5,b=125:g,8a#.,16g,16p,16g,8c6,8g,8f,g,8d.6,16g,16p,16g,8d#6,8d6,8a#,8g,8d6,8g6,16g,16f,16p,16f,8d,8a#,2g,p,16f6,8d6,8c6,8a#,g,8a#.,16g,16p,16g,8c6,8g,8f,g,8d.6,16g,16p,16g,8d#6,8d6,8a#,8g,8d6,8g6,16g,16f,16p,16f,8d,8a#,2g",
            "batman:d=8,o=5,b=180:b,b,a#,a#,a,a,a#,a#,b,b,a#,a#,a,a,a#,a#,4b,p,4b",
            "The X Files:d=4,o=5,b=112:16c,16d#,16g,8g#,2p,8p,c,c,c,c,g,f,g,a#,16g,16d#,16g,8g#,2p,p,2d.6,d#6,d6,c6,a#,d6,2g.,d#6,d6,c6,a#,d6,1g,16c,16d#,16g,8g#,2p,p,c,c,c"
        };


        private string PickRandomSong()
        {
            var index = _random.Next(Songs.Length);

            return Songs[index];
        }

        private void PlayTunes()
        {
            while (true)
            {
                var nextSong = PickRandomSong();
                if (Rtttl.TryParse(nextSong.AsSpan(), out var rtttl))
                {
                    rtttl.Play(new MeadowPwmPlayer(_pwmBuzzer));
                }

                Thread.Sleep(10000);
            }
        }

        public class MeadowPwmPlayer : FrequencyRtttlPlayer
        {
            private readonly IPwmPort _pwm;

            public MeadowPwmPlayer(IPwmPort pwm)
            {
                _pwm = pwm;
            }

            protected override void Play(double frequency, TimeSpan duration)
            {
                if (frequency == 0)
                {
                    _pwm.Stop();
                }
                else
                {
                    _pwm.Frequency = (float)frequency;
                    _pwm.Start();
                }
                Thread.Sleep(duration);
            }
        }
    }
}
