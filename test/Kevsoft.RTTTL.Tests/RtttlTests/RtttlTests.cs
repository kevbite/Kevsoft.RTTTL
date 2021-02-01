using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Kevsoft.RTTTL.Tests.RtttlTests
{
    public class RtttlTests
    {
        [Fact]
        public void EmptyText()
        {
            var result = Rtttl.TryParse("::", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().BeEmpty();
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = Duration.Four,
                    Scale = Scale.Six,
                    BeatsPerMinute = 63
                });
        }

        [Fact]
        public void SimpsonsText()
        {
            var result = Rtttl.TryParse("Simpsons:d=4,o=5,b=160:32p,c.6,e6,f#6,8a6,g.6,e6,c6,8a,8f#,8f#,8f#,2g",
                out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().Be("Simpsons");
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = Duration.Four,
                    Scale = Scale.Five,
                    BeatsPerMinute = 160
                });
            rtttl.Notes.Should().BeEquivalentTo(new[]
            {
                new Note(Pitch.Pause, Duration.ThirtyTwo, null, false), //32p,
                new Note(Pitch.C, null, Scale.Six, true), //c.6,
                new Note(Pitch.E, null, Scale.Six, false), //e6,
                new Note(Pitch.FSharp, null, Scale.Six, false), //f#6,
                new Note(Pitch.A, Duration.Eight, Scale.Six, false), //8a6,
                new Note(Pitch.G, null, Scale.Six, true), //g.6,
                new Note(Pitch.E, null, Scale.Six, false), //e6,
                new Note(Pitch.C, null, Scale.Six, false), //c6,
                new Note(Pitch.A, Duration.Eight, null, false), //8a,
                new Note(Pitch.FSharp, Duration.Eight, null, false), //8f#,
                new Note(Pitch.FSharp, Duration.Eight, null, false), //8f#,
                new Note(Pitch.FSharp, Duration.Eight, null, false), //8f#,
                new Note(Pitch.G, Duration.Two, null, false), //2g
            }, options => options.WithStrictOrdering());
        }
    }
}