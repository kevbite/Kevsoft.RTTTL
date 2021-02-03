using System;

namespace Kevsoft.RTTTL
{
    public abstract class FrequencyRtttlPlayer : IRtttlPlayer
    {
        public void PlayNote(Pitch pitch, Scale scale, TimeSpan duration)
        {
            var frequency = CalculateFrequency(pitch, scale);

            Play(frequency, duration);
        }

        protected abstract void Play(double frequency, TimeSpan duration);

        private double CalculateFrequency(Pitch pitch, Scale scale)
        {
            if (pitch == Pitch.Pause)
            {
                return 0;
            }

            var c4 = 261.63d;
            var twelfthRoot = Math.Pow(2, (double) 1 / 12);
            var semitones = CalculateSemitonesFromC4(pitch, scale);
            var frequency = c4 * Math.Pow(twelfthRoot, semitones);

            return Math.Round(frequency * 1e1) / 1e1;
        }

        private static double CalculateSemitonesFromC4(Pitch note, Scale scale)
        {
            var noteOrder = new[]
            {
                Pitch.C,
                Pitch.CSharp,
                Pitch.D,
                Pitch.DSharp,
                Pitch.E,
                Pitch.F,
                Pitch.FSharp,
                Pitch.G,
                Pitch.GSharp,
                Pitch.A,
                Pitch.ASharp,
                Pitch.B
            };
            var middleScale = 4;
            var semitonesInScale = 12;

            var scaleJump = ((int) scale - middleScale) * semitonesInScale;

            return Array.IndexOf(noteOrder, note) + scaleJump;
        }
    }
}