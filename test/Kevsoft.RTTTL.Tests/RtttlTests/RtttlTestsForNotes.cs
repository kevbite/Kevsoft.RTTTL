using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Kevsoft.RTTTL.Tests.RtttlTests
{
    public class RtttlTestsForNotes
    {
        [Theory]
        [InlineData("p", Pitch.Pause)]
        [InlineData("c", Pitch.C)]
        [InlineData("c#", Pitch.CSharp)]
        [InlineData("d", Pitch.D)]
        [InlineData("d#", Pitch.DSharp)]
        [InlineData("e", Pitch.E)]
        [InlineData("f", Pitch.F)]
        [InlineData("f#", Pitch.FSharp)]
        [InlineData("g", Pitch.G)]
        [InlineData("g#", Pitch.GSharp)]
        [InlineData("a", Pitch.A)]
        [InlineData("a#", Pitch.ASharp)]
        [InlineData("b", Pitch.B)]
        public void OnlySingleNotePitchText(string note, Pitch expectedPitch)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Notes.Should().HaveCount(1)
                .And.BeEquivalentTo(new
                {
                    Pitch = expectedPitch
                });
        }

        [Theory]
        [InlineData("1p", Pitch.Pause, Duration.One)]
        [InlineData("2p", Pitch.Pause, Duration.Two)]
        [InlineData("4p", Pitch.Pause, Duration.Four)]
        [InlineData("8p", Pitch.Pause, Duration.Eight)]
        [InlineData("16p", Pitch.Pause, Duration.Sixteen)]
        [InlineData("32p", Pitch.Pause, Duration.ThirtyTwo)]
        [InlineData("1c#", Pitch.CSharp, Duration.One)]
        [InlineData("2c#", Pitch.CSharp, Duration.Two)]
        [InlineData("4c#", Pitch.CSharp, Duration.Four)]
        [InlineData("8c#", Pitch.CSharp, Duration.Eight)]
        [InlineData("16c#", Pitch.CSharp, Duration.Sixteen)]
        [InlineData("32c#", Pitch.CSharp, Duration.ThirtyTwo)]
        public void OnlySingleNoteWithDurationAndPitchText(string note, Pitch expectedPitch, Duration expectedDuration)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Notes.Should().HaveCount(1)
                .And.BeEquivalentTo(new
                {
                    Pitch = expectedPitch,
                    Duration = expectedDuration
                });
        }

        [Theory]
        [InlineData("p4", Pitch.Pause, Scale.Four)]
        [InlineData("p5", Pitch.Pause, Scale.Five)]
        [InlineData("p6", Pitch.Pause, Scale.Six)]
        [InlineData("p7", Pitch.Pause, Scale.Seven)]
        [InlineData("c#4", Pitch.CSharp, Scale.Four)]
        [InlineData("c#5", Pitch.CSharp, Scale.Five)]
        [InlineData("c#6", Pitch.CSharp, Scale.Six)]
        [InlineData("c#7", Pitch.CSharp, Scale.Seven)]
        public void OnlySingleNoteWithPitchAndScaleText(string note, Pitch expectedPitch, Scale expectedScale)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Notes.Should().HaveCount(1)
                .And.BeEquivalentTo(new
                {
                    Pitch = expectedPitch,
                    Scale = expectedScale
                });
        }

        [Theory]
        [InlineData("p.", Pitch.Pause)]
        [InlineData("c#.", Pitch.CSharp)]
        public void OnlySingleNoteWithPitchAndDotted(string note, Pitch expectedPitch)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Notes.Should().HaveCount(1)
                .And.BeEquivalentTo(new
                {
                    Pitch = expectedPitch,
                    Dotted = true
                });
        }

        [Fact]
        public void OnlyMultipleNotes()
        {
            var notes = "a,2e,d#,b4,2a4,2e.";
            var result = Rtttl.TryParse($"::{notes}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Notes.Should().HaveCount(6)
                .And.BeEquivalentTo(new[]
                    {
                        new Note(Pitch.A, null, null, false),
                        new Note(Pitch.E, Duration.Two, null, false),
                        new Note(Pitch.DSharp, null, null, false),
                        new Note(Pitch.B, null, Scale.Four, false),
                        new Note(Pitch.A, Duration.Two, Scale.Four, false),
                        new Note(Pitch.E, Duration.Two, null, true)
                    },
                    options => options.WithStrictOrdering());
        }

        [Theory]
        [InlineData("h")]
        [InlineData("i")]
        [InlineData("j")]
        [InlineData("k")]
        [InlineData("x")]
        [InlineData("y")]
        [InlineData("z")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void OnlySingleInvalidNotePitchText(string note)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }

        [Theory]
        [InlineData("p#")]
        [InlineData("e#")]
        [InlineData("b#")]
        public void OnlySingleInvalidNotePitchSharpText(string note)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }

        [Theory]
        [InlineData("pjunk")]
        [InlineData("p4junk")]
        [InlineData("p4.junk")]
        [InlineData("p.junk")]
        [InlineData("c#junk")]
        [InlineData("c#4junk")]
        [InlineData("c#4.junk")]
        [InlineData("c#.junk")]
        public void OnlySingleInvalidNoteWithTrailingJunkText(string note)
        {
            var result = Rtttl.TryParse($"::{note}", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }
    }
}