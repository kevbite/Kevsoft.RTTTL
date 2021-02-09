using System;
using System.Diagnostics.CodeAnalysis;

namespace Kevsoft.RTTTL
{
    public sealed class RtttlSettings
    {
        internal const Duration DefaultDuration = Duration.Four;
        internal const Scale DefaultScale = Scale.Six;
        internal const byte DefaultBeatsPerMinute = 63;
        internal const int MillisecondsPerMinute = 60000;

        public RtttlSettings(Duration duration, Scale scale, byte beatsPerMinute)
        {
            Duration = duration;
            Scale = scale;
            BeatsPerMinute = beatsPerMinute;
        }

        internal RtttlSettings With(Duration duration) => new RtttlSettings(duration, Scale, BeatsPerMinute);
        internal RtttlSettings With(Scale scale) => new RtttlSettings(Duration, scale, BeatsPerMinute);
        internal RtttlSettings With(byte beatsPerMinute) => new RtttlSettings(Duration, Scale, beatsPerMinute);

        public Duration Duration { get; }
        public Scale Scale { get; }
        public byte BeatsPerMinute { get; }

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

            var key = current.Slice(0, indexOfKeyValueSplit);
            var value = current.Slice(indexOfKeyValueSplit + 1);

            if (key is { Length: 1 })
            {
                if (key[0] == 'd' && DurationEnumHelper.TryParseDefinedDuration(value, out var duration) && duration.HasValue)
                {
                    valueOut = settings.With(duration.Value);
                    return true;
                }

                if (key[0] == 'o' && ScaleEnumHelper.TryParseDefinedScale(value, out var scale))
                {
                    valueOut = settings.With(scale);
                    return true;
                }


                if (key[0] == 'b')
                {
                    if (
#if NETSTANDARD2_1
                            byte.TryParse(value, out var bpm)
#else
                            byte.TryParse(new string(value.ToArray()), out var bpm)
#endif
                            && bpm != 0)
                    {
                        valueOut = settings.With(bpm);

                        return true;
                    }
                }

            }

            return false;
        }

    }
}