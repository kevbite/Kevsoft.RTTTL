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
    }
}