using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kevsoft.RTTTL
{
    public class Rtttl
    {
        private Rtttl(string name, RtttlSettings settings, IReadOnlyCollection<Note> notes)
        {
            Name = name;
            Settings = settings;
            Notes = notes;
        }

        public static bool TryParse(ReadOnlySpan<char> text, [MaybeNullWhen(returnValue: false)] out Rtttl rtttl)
        {
            rtttl = null;

            var endOfName = text.IndexOf(RtttlSettings.Separator);
            var name = new string(text[..endOfName]);

            text = text[(endOfName + 1)..];

            var endOfSettings = text.IndexOf(RtttlSettings.Separator);

            if (!TryParseRtttlSettings(text[..endOfSettings], out var rtttlSettings))
                return false;

            if (!TryParseNotes(text[(endOfSettings+1)..], out var notes))
                return false;

            rtttl = new Rtttl(name, rtttlSettings, notes);

            return true;
        }

        private static bool TryParseNotes(ReadOnlySpan<char> text, [MaybeNullWhen(returnValue: false)] out IReadOnlyCollection<Note> notes)
        {
            notes = null;
            var parsedNotes = new List<Note>();
            while (!text.IsEmpty)
            {
                text = ConsumeToAndEatDelimiter(text, ',', out var current);

                if (!TryParseNote(current, out var note))
                {
                    return false;
                }

                parsedNotes.Add(note);

            }
            
            notes = parsedNotes.AsReadOnly();
            return true;
        }

        private static bool TryParseNote(ReadOnlySpan<char> current, [MaybeNullWhen(returnValue: false)]out Note note)
        {
            note = null;
            if (PitchMap.TryGetValue(current.ToString(), out var pitch))
            {
                note = new Note(pitch);
                return true;
            }

            return false;
        }

        private static readonly IDictionary<string, Pitch> PitchMap = new Dictionary<string, Pitch>
        {
            ["p"] = Pitch.Pause,
            ["c"] = Pitch.C,
            ["c#"] = Pitch.CSharp,
            ["d"] = Pitch.D,
            ["d#"] = Pitch.DSharp,
            ["e"] = Pitch.E,
            ["f"] = Pitch.F,
            ["f#"] = Pitch.FSharp,
            ["g"] = Pitch.G,
            ["g#"] = Pitch.GSharp,
            ["a"] = Pitch.A,
            ["a#"] = Pitch.ASharp,
            ["b"] = Pitch.B
        };

        private static bool TryParseRtttlSettings(ReadOnlySpan<char> text,
            [MaybeNullWhen(returnValue: false)] out RtttlSettings rtttlSettings)
        {
            rtttlSettings = null;

            var settings = RtttlSettings.Default();

            while (!text.IsEmpty)
            {
                text = ConsumeToAndEatDelimiter(text, ',', out var current);
         
                if (!TryParseSettingKeyValue(current, settings, out settings))
                {
                    return false;
                }
            }

            rtttlSettings = settings;
            return true;
        }
        
        private static ReadOnlySpan<char> ConsumeToAndEatDelimiter(ReadOnlySpan<char> text, char delimiter, out ReadOnlySpan<char> value)
        {
            var indexOfNext = text.IndexOf(delimiter);
            
            if (indexOfNext is -1)
            {
                value = text[..text.Length];
                text = ReadOnlySpan<char>.Empty;
            }
            else
            {
                value = text[..indexOfNext];
                text = text[(indexOfNext + 1)..];
            }

            return text;
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
                if (key[0] == 'd' && Enum.TryParse<Duration>(new string(value), out var duration) &&
                    Enum.IsDefined(duration))
                {
                    valueOut = settings with {Duration = duration};
                    return true;
                }
                if (key[0] == 'o' && Enum.TryParse<Scale>(new string(value), out var scale) &&
                    Enum.IsDefined(scale))
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

        public string Name { get; }
        public RtttlSettings Settings { get; }
        public IReadOnlyCollection<Note> Notes { get; }
    }

    public class Note
    {
        public Pitch Pitch { get; }

        public Note(Pitch pitch)
        {
            Pitch = pitch;
        }
    }

    public enum Pitch
    {
        Pause,
        A,
        ASharp,
        B,
        C,
        CSharp,
        D,
        DSharp,
        E,
        F,
        FSharp,
        G,
        GSharp
    }

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