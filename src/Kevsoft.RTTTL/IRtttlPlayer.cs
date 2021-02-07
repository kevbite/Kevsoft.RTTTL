using System;

namespace Kevsoft.RTTTL
{
    public interface IRtttlPlayer
    {
        void PlayNote(Pitch pitch, Scale scale, TimeSpan duration);
    }
}