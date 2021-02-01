using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Kevsoft.RTTTL.Tests.RtttlTests
{
    public class RtttlTestsForSettings
    {
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

        [Theory]
        [InlineData("-1")]
        [InlineData("3")]
        [InlineData("abc")]
        [InlineData("300")]
        [InlineData("")]
        public void OnlyInvalidDurationSettingText(string duration)
        {
            var result = Rtttl.TryParse($":d={duration}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }

        [Theory]
        [InlineData("4", Scale.Four)]
        [InlineData("5", Scale.Five)]
        [InlineData("6", Scale.Six)]
        [InlineData("7", Scale.Seven)]
        public void OnlyScaleSettingText(string scale, Scale expectedScale)
        {
            var result = Rtttl.TryParse($":o={scale}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().BeEmpty();
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = Duration.Four,
                    Scale = expectedScale,
                    BeatsPerMinute = 63
                });
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("8")]
        [InlineData("9")]
        [InlineData("abc")]
        [InlineData("")]
        public void OnlyInvalidScaleSettingText(string scale)
        {
            var result = Rtttl.TryParse($":o={scale}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }

        [Theory]
        [InlineData("40", 40)]
        [InlineData("50", 50)]
        [InlineData("60", 60)]
        public void OnlyBeatsPerMinuteSettingText(string bpm, int expectedbpm)
        {
            var result = Rtttl.TryParse($":b={bpm}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().BeEmpty();
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = Duration.Four,
                    Scale = Scale.Six,
                    BeatsPerMinute = expectedbpm
                });
        }

        [Theory]
        [InlineData("-30")]
        [InlineData("0")]
        [InlineData("abc")]
        [InlineData("")]
        public void OnlyInvalidBeatsPerMinuteSettingText(string bpm)
        {
            var result = Rtttl.TryParse($":b={bpm}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(false);
            rtttl.Should().BeNull();
        }

        [Theory]
        [InlineData("d=4,o=5,b=160", Duration.Four, Scale.Five, 160)]
        [InlineData("d=4,o=5,b=108", Duration.Four, Scale.Five, 108)]
        [InlineData("d=8,o=7,b=60", Duration.Eight, Scale.Seven, 60)]
        public void MultipleSettingText(string settingsText, Duration duration, Scale scale, int beatsPerMinute)
        {
            var result = Rtttl.TryParse($":{settingsText}:", out var rtttl);

            using var _ = new AssertionScope();
            result.Should().Be(true);
            rtttl!.Name.Should().BeEmpty();
            rtttl.Settings.Should()
                .BeEquivalentTo(new
                {
                    Duration = duration,
                    Scale = scale,
                    BeatsPerMinute = beatsPerMinute
                });
        }
    }
}