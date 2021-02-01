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
    }
}