using System;
using System.Diagnostics.CodeAnalysis;

namespace Kevsoft.RTTTL
{
    public record RtttlSettings(Duration Duration, Scale Scale, byte BeatsPerMinute)
    {
        public const Duration DefaultDuration = Duration.Four;
        public const Scale DefaultScale = Scale.Six;
        public const byte DefaultBeatsPerMinute = 63;
        public static RtttlSettings Default()
        {
            var duration = DefaultDuration;
            var scale = DefaultScale;
            var beatsPerMinute = DefaultBeatsPerMinute;

            return new RtttlSettings(
                duration,
                scale,
                beatsPerMinute
            );
        }

            internal static bool TryParse(ReadOnlySpan<char> text,
                [MaybeNullWhen(returnValue: false)] out RtttlSettings rtttlSettings)
            {
                rtttlSettings = null;

                var settings = RtttlSettings.Default();

                while (!text.IsEmpty)
                {
                    text = text.ConsumeToAndEatDelimiter(',', out var current);

                    if (!TryParseSettingKeyValue(current, settings, out settings))
                    {
                        return false;
                    }
                }

                rtttlSettings = settings;
                return true;
            }
        
            private static bool TryParseSettingKeyValue(ReadOnlySpan<char> current, RtttlSettings settings,
                [MaybeNullWhen(returnValue: false)] out RtttlSettings valueOut)
            {
                valueOut = null;
                var indexOfKeyValueSplit = current.IndexOf('=');

                var key = current[..indexOfKeyValueSplit];
                var value = current[(indexOfKeyValueSplit + 1)..];

                if (key is {Length: 1})
                {
                    if (key[0] == 'd' && DurationEnumHelper.TryParseDefinedDuration(value, out var duration) && duration.HasValue)
                    {
                        valueOut = settings with {Duration = duration.Value};
                        return true;
                    }

                    if (key[0] == 'o' && ScaleEnumHelper.TryParseDefinedScale(value, out var scale))
                    {
                        valueOut = settings with {Scale = scale};
                        return true;
                    }

                    if (key[0] == 'b' && byte.TryParse(value, out var bpm) && bpm != 0)
                    {
                        valueOut = settings with {BeatsPerMinute = bpm};
                        return true;
                    }
                }

                return false;
            }

    }
}