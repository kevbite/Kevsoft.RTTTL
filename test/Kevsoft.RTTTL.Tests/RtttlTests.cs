using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Kevsoft.RTTTL.Tests
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
        
        [Theory]
        [InlineData("1", Duration.One)]
        [InlineData("2", Duration.Two)]
        [InlineData("4", Duration.Four)]
        [InlineData("8", Duration.Eight)]
        [InlineData("16", Duration.Sixteen)]
        [InlineData("32", Duration.ThirtyTwo)]
        public void OnlyDurationSettingText(string duration, Duration expectedDuration)
        {
            var result = Rtttl.TryParse($":d={duration}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().BeEmpty();
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = expectedDuration,
                    Scale = Scale.Six,
                    BeatsPerMinute = 63
                });
        }
    }
}
