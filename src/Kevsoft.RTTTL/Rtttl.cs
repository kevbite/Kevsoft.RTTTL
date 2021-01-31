using System;

namespace Kevsoft.RTTTL
{
    public class Rtttl
    {
        private Rtttl(string name, RtttlSettings settings)
        {
            Name = name;
            Settings = settings;
        }
        public static bool TryParse(ReadOnlySpan<char> text, out Rtttl? rtttl)
        {
            rtttl = null;
            var duration = RtttlSettings.DefaultDuration;

            var endOfName = text.IndexOf(RtttlSettings.Separator);
            var name = new string(text.Slice(0, endOfName));

            text = text.Slice(endOfName + 1);
            while (text[0] != RtttlSettings.Separator)
            {
                var endOfKey = text.IndexOf('=');
                var key = text.Slice(0, endOfKey);
                var indexOf = text.IndexOf(',');
                var indexOf2 = text.IndexOf(':');
                var endOfValue = indexOf != -1 ? indexOf : indexOf2;
                var length = endOfValue-2;
                var value = text.Slice(endOfKey+1, length);

                if (key[0] is 'd')
                {
                    if ((int.TryParse(value, out var v), Enum.IsDefined(typeof(Duration), v)) is not (true,true))
                    {
                        return false;
                    }
                    duration = (Duration) v;
                }

                text = text.Slice(endOfValue);
            }


            rtttl = new Rtttl(name, new RtttlSettings(
                duration,
                RtttlSettings.DefaultScale,
                RtttlSettings.DefaultBeatsPerMinute
            ));
            
            return true;
        }

        public string Name { get; }
        public RtttlSettings Settings { get; }
    }

    public class RtttlSettings
    {
        public const Duration DefaultDuration = Duration.Four;
        public const Scale DefaultScale = Scale.Six;
        public const int DefaultBeatsPerMinute = 63;
        public const char Separator = ':';
        
        public RtttlSettings(Duration duration, Scale scale, int beatsPerMinute)
        {
            Duration = duration;
            Scale = scale;
            BeatsPerMinute = beatsPerMinute;
        }

        public Duration Duration { get; }
        public Scale Scale { get; }
        public int BeatsPerMinute { get; }
    }

    public enum Duration
    {
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8,
        Sixteen = 16,
        ThirtyTwo = 32
    }

    public enum Scale
    {
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7
    }
}