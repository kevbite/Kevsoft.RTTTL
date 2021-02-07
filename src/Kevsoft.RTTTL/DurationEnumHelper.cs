using System;
using System.Diagnostics.CodeAnalysis;

namespace Kevsoft.RTTTL
{
    internal static class DurationEnumHelper
    {
        internal static bool TryParseDefinedDuration(ReadOnlySpan<char> value, [MaybeNullWhen(returnValue: false)] out Duration? duration)
        {
            duration = null;
#if NETSTANDARD2_1
            var s = new string(value);
#else
            var s = new string(value.ToArray());
#endif
            if (Enum.TryParse<Duration>(s, out var parsed) &&
                Enum.IsDefined(typeof(Duration), parsed))
            {
                duration = parsed;
                return true;
            }

            return false;
        }
    }
}