using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Kevsoft.RTTTL
{
    public sealed class Rtttl
    {
        public const char Separator = ':';

        public Rtttl(string name, RtttlSettings settings, IReadOnlyCollection<Note> notes)
        {
            Name = name;
            Settings = settings;
            Notes = notes;
        }

        public static bool TryParse(ReadOnlySpan<char> text, [MaybeNullWhen(returnValue: false)] out Rtttl rtttl)
        {
            rtttl = null;

            var endOfName = text.IndexOf(Separator);
            var name = new string(text[..endOfName]);

            text = text[(endOfName + 1)..];

            var endOfSettings = text.IndexOf(Separator);

            if (!RtttlSettings.TryParse(text[..endOfSettings], out var rtttlSettings))
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
                text = text.ConsumeToAndEatDelimiter(',', out var current);

                if (!Note.TryParseNote(current, out var note))
                {
                    return false;
                }

                parsedNotes.Add(note);
            }

            notes = parsedNotes.AsReadOnly();
            return true;
        }

        public void Play(IRtttlPlayer player)
        {
            var beatEvery = (double)RtttlSettings.MillisecondsPerMinute / Settings.BeatsPerMinute;
            
            TimeSpan CalculateDuration(Duration noteDuration, bool isDotted) {
                var duration = (beatEvery * 4) / (int)noteDuration;
                var prolonged = isDotted ? (duration / 2) : 0;
                return TimeSpan.FromMilliseconds(duration + prolonged);
            }
            
            foreach (var note in Notes)
            {
                var duration = CalculateDuration(
                    note.Duration ?? Settings.Duration, note.Dotted);

                player.PlayNote(note.Pitch, note.Scale ?? Settings.Scale, duration);
            }
        }
      

        public string Name { get; }
        public RtttlSettings Settings { get; }
        public IReadOnlyCollection<Note> Notes { get; }
    }

    public interface IRtttlPlayer
    {
        void PlayNote(Pitch pitch, Scale scale, TimeSpan duration);
    }
}