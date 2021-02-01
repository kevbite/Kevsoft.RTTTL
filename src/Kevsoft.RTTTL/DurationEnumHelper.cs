using System;
using System.Diagnostics.CodeAnalysis;

namespace Kevsoft.RTTTL
{
    internal static class DurationEnumHelper
    {
        internal static bool TryParseDefinedDuration(ReadOnlySpan<char> value, [MaybeNullWhen(returnValue: false)] out Duration? duration)
        {
            duration = null;
            if (Enum.TryParse<Duration>(new string(value), out var parsed) &&
                Enum.IsDefined(parsed))
            {
                duration = parsed;
                return true;
            }

            return false;
        }
    }
}