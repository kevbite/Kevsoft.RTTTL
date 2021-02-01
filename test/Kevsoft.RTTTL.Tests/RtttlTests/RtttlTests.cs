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
    }
}
