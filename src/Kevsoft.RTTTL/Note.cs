using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kevsoft.RTTTL
{
    public class Note
    {
        public Pitch Pitch { get; }
        public Duration? Duration { get; }
        public Scale? Scale { get; }
        public bool Dotted { get; }

        public Note(Pitch pitch, Duration? duration, Scale? scale, bool dotted)
        {
            Pitch = pitch;
            Duration = duration;
            Scale = scale;
            Dotted = dotted;
        }
        
        
        internal static bool TryParseNote(ReadOnlySpan<char> current, [MaybeNullWhen(returnValue: false)] out Note note)
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
                if (!ScaleEnumHelper.TryParseDefinedScale(current[..1], out var parsedScale))
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
        
        private static bool TryParseNoteDuration(ReadOnlySpan<char> value, out Duration? duration)
        {
            duration = null;

            if (value.IsEmpty)
            {
                return true;
            }

            if (DurationEnumHelper.TryParseDefinedDuration(value, out var d))
            {
                duration = d;
                return true;
            }

            return false;
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

    }
}