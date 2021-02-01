using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Kevsoft.RTTTL.Tests.RtttlTests
{
    public class RtttlTestsForName
    {
        [Fact]
        public void OnlyNameText()
        {
            var result = Rtttl.TryParse("HauntHouse::", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().Be("HauntHouse");
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = Duration.Four,
                    Scale = Scale.Six,
                    BeatsPerMinute = 63
                });
        }
    }
}