namespace Kevsoft.RTTTL
{
    public record RtttlSettings(Duration Duration, Scale Scale, byte BeatsPerMinute)
    {
        public const Duration DefaultDuration = Duration.Four;
        public const Scale DefaultScale = Scale.Six;
        public const byte DefaultBeatsPerMinute = 63;
        public const char Separator = ':';

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
    }
}