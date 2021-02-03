using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Kevsoft.RTTTL.Tests
{
    public class FrequencyRtttlPlayerTests : FrequencyRtttlPlayer
    {
        private readonly List<(double frequency, TimeSpan duration)> _captured = new();
        protected override void Play(double frequency, TimeSpan duration)
        {
            _captured.Add((frequency,duration));
        }

        [Fact]
        public void PlaysCorrectFrequenciesForScales()
        {
            // https://www.intmath.com/trigonometric-graphs/music.php
            PlayNote(Pitch.C, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.CSharp, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.D, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.DSharp, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.E, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.F, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.FSharp, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.G, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.GSharp, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.A, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.ASharp, Scale.Five, TimeSpan.FromSeconds(1));
            PlayNote(Pitch.B, Scale.Five, TimeSpan.FromSeconds(1));

            _captured.Should()
                .BeEquivalentTo(new[]
                {
                    (523.25, TimeSpan.FromSeconds(1)),
                    (554.37, TimeSpan.FromSeconds(1)),
                    (587.33, TimeSpan.FromSeconds(1)),
                    (622.25, TimeSpan.FromSeconds(1)),
                    (659.26, TimeSpan.FromSeconds(1)),
                    (698.46, TimeSpan.FromSeconds(1)),
                    (739.99, TimeSpan.FromSeconds(1)),
                    (783.99, TimeSpan.FromSeconds(1)),
                    (830.61, TimeSpan.FromSeconds(1)),
                    (880, TimeSpan.FromSeconds(1)),
                    (932.33, TimeSpan.FromSeconds(1)),
                    (987.77, TimeSpan.FromSeconds(1))
                }, options => options.WithStrictOrdering()
                    .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.1))
                    .WhenTypeIs<double>());
        }
    }
}