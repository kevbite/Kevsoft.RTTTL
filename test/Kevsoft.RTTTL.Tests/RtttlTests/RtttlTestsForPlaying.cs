using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Kevsoft.RTTTL.Tests.RtttlTests
{
    public class RtttlTestsForPlaying : IRtttlPlayer
    {
        private readonly List<(Pitch, Scale, TimeSpan)> _capturedPlayedNotes = new();

        [Theory]
        [InlineData(Pitch.A, Scale.Five)]
        [InlineData(Pitch.C, Scale.Six)]
        public void PlayingSingleNoteWithDefaults(Pitch pitch, Scale scale)
        {
            var rtttlSettings = new RtttlSettings(Duration.Sixteen, scale, 200);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(pitch, null, null, false)
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(1)
                .And
                .BeEquivalentTo((pitch, scale, TimeSpan.FromMilliseconds(75)));
        }

        [Theory]
        [InlineData(Pitch.A, Scale.Five)]
        [InlineData(Pitch.C, Scale.Six)]
        public void PlayingSingleNoteWithScale(Pitch pitch, Scale scale)
        {
            var rtttlSettings = new RtttlSettings(Duration.Sixteen, Scale.Four, 200);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(pitch, null, scale, false)
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(1)
                .And
                .BeEquivalentTo((pitch, scale, TimeSpan.FromMilliseconds(75)));
        }

        [Theory]
        [InlineData(Pitch.A)]
        [InlineData(Pitch.C)]
        public void PlayingSingleNoteWithDuration(Pitch pitch)
        {
            var rtttlSettings = new RtttlSettings(Duration.One, Scale.Four, 200);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(pitch, Duration.Sixteen, null, false)
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(1)
                .And
                .BeEquivalentTo((pitch, Scale.Four, TimeSpan.FromMilliseconds(75)));
        }

        [Fact]
        public void PlayingSingleNoteWithDurationDotted()
        {
            var rtttlSettings = new RtttlSettings(Duration.One, Scale.Four, 120);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(Pitch.A, Duration.Eight, null, true)
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(1)
                .And
                .BeEquivalentTo((Pitch.A, Scale.Four, TimeSpan.FromMilliseconds(375)));
        }

        [Fact]
        public void PlayingSingleNoteWithDefaultsDotted()
        {
            var rtttlSettings = new RtttlSettings(Duration.Eight, Scale.Four, 120);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(Pitch.A, null, null, true)
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(1)
                .And
                .BeEquivalentTo((Pitch.A, Scale.Four, TimeSpan.FromMilliseconds(375)));
        }

        [Fact]
        public void PlayingMultipleNotes()
        {
            var rtttlSettings = new RtttlSettings(Duration.Sixteen, Scale.Four, 120);
            var rtttl = new Rtttl("Test", rtttlSettings, new[]
            {
                new Note(Pitch.A, null, Scale.Four, false),
                new Note(Pitch.B, null, Scale.Five, false),
                new Note(Pitch.C, null, Scale.Six, false),
                new Note(Pitch.D, null, Scale.Seven, false),
            });

            rtttl.Play(this);

            _capturedPlayedNotes.Should()
                .HaveCount(4)
                .And
                .BeEquivalentTo(new[]
                {
                    (Pitch.A, Scale.Four, TimeSpan.FromMilliseconds(125)),
                    (Pitch.B, Scale.Five, TimeSpan.FromMilliseconds(125)),
                    (Pitch.C, Scale.Six, TimeSpan.FromMilliseconds(125)),
                    (Pitch.D, Scale.Seven, TimeSpan.FromMilliseconds(125))
                }, options => options.WithStrictOrdering());
        }

        void IRtttlPlayer.PlayNote(Pitch pitch, Scale scale, TimeSpan duration)
        {
            _capturedPlayedNotes.Add((pitch, scale, duration));
        }
    }
}