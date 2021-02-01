using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

            if (!TryParseNotes(text[(endOfSettings + 1)..], out var notes))
                return false;

            rtttl = new Rtttl(name, rtttlSettings, notes);

            return true;
        }

        private static bool TryParseNotes(ReadOnlySpan<char> text,
            [MaybeNullWhen(returnValue: false)] out IReadOnlyCollection<Note> notes)
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

        private static bool TryParseNote(ReadOnlySpan<char> current, [MaybeNullWhen(returnValue: false)] out Note note)
        {
            note = null;
            current = current.Trim();
            var indexOfStartPitch = current.IndexOfAny(new Span<char>("pcdefgab".ToCharArray()));

            if (indexOfStartPitch == -1 || !TryParseNoteDuration(current[..indexOfStartPitch], out var duration))
            {
                return false;
            }

            current = current[indexOfStartPitch..];

            if (!PitchMap.TryGetValue(current[0], out var pitch))
            {
                return false;
            }

            current = current[1..];

            if (!current.IsEmpty && current[0] is '#')
            {
                if (!TrySharpen(pitch, out pitch))
                {
                    return false;
                }
                current = current[1..];
            }

            var dotted = false;
            if (!current.IsEmpty && current[0] == '.')
            {
                dotted = true;
                current = current[1..];
            }
            
            Scale? scale = null;
            if (!current.IsEmpty && PossibleScales.Contains(current[0]))
            {
                if (!TryParseScale(current[..1], out var parsedScale))
                {
                    return false;
                }

                scale = parsedScale;
                current = current[1..];
            }

            if (!current.IsEmpty)
            {
                return false;
            }
            
            note = new Note(pitch, duration, scale, dotted);

            return true;
        }

        private static bool TrySharpen(Pitch value, out Pitch sharpenedPitch)
        {
            var sharpened = value switch
            {
                Pitch.C => Pitch.CSharp,
                Pitch.D => Pitch.DSharp,
                Pitch.F =>  Pitch.FSharp,
                Pitch.G =>  Pitch.GSharp,
                Pitch.A => Pitch.ASharp,
                _ => (Pitch?)null
            };

            sharpenedPitch = sharpened ?? value;
            
            return sharpened.HasValue;
        }

        private static readonly IDictionary<char, Pitch> PitchMap = new Dictionary<char, Pitch>
        {
            ['p'] = Pitch.Pause,
            ['c'] = Pitch.C,
            ['d'] = Pitch.D,
            ['e'] = Pitch.E,
            ['f'] = Pitch.F,
            ['g'] = Pitch.G,
            ['a'] = Pitch.A,
            ['b'] = Pitch.B
        };

        private static readonly HashSet<char> PossibleScales = Enum.GetValues<Scale>().Select(x => $"{x:D}"[0]).ToHashSet();

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

        private static ReadOnlySpan<char> ConsumeToAndEatDelimiter(ReadOnlySpan<char> text, char delimiter,
            out ReadOnlySpan<char> value)
        {
            var indexOfDelimiter = text.IndexOf(delimiter);

            if (indexOfDelimiter is -1)
            {
                value = text[..text.Length];
                text = ReadOnlySpan<char>.Empty;
            }
            else
            {
                value = text[..indexOfDelimiter];
                text = text[(indexOfDelimiter + 1)..];
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
                if (key[0] == 'd' && TryParseNoteDuration(value, out var duration) && duration.HasValue)
                {
                    valueOut = settings with {Duration = duration.Value};
                    return true;
                }

                if (key[0] == 'o' && TryParseScale(value, out var scale))
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

        private static bool TryParseScale(ReadOnlySpan<char> value, out Scale scale)
        {
            return Enum.TryParse(new string(value), out scale) &&
                Enum.IsDefined(scale);
        }

        private static bool TryParseNoteDuration(ReadOnlySpan<char> value, out Duration? duration)
        {
            duration = null;

            if (value.IsEmpty)
            {
                return true;
            }

            if (Enum.TryParse<Duration>(new string(value), out var parsed) &&
                Enum.IsDefined(parsed))
            {
                duration = parsed;
                return true;
            }

            return false;
        }

        public string Name { get; }
        public RtttlSettings Settings { get; }
        public IReadOnlyCollection<Note> Notes { get; }
    }
}